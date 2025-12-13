using AutoMapper;
using Azure.Core;
using ES.Academics.Application.Academics.Command.AddExamResult;
using ES.Academics.Application.Academics.Command.AddExamSession;
using ES.Academics.Application.Academics.Command.AddSeatPlanning;
using ES.Academics.Application.Academics.Command.AddSubject;
using ES.Academics.Application.Academics.Queries.ClassByExamSession;
using ES.Academics.Application.Academics.Queries.Exam;
using ES.Academics.Application.Academics.Queries.FilterExamSession;
using ES.Academics.Application.Academics.Queries.FilterSubject;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.Repository.HelperServices;

namespace ES.Academics.Infrastructure.ServiceImpl
{
    public class SeatPlanningServices : ISeatPlanningServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        private readonly ILogger<SeatPlanningServices> _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SeatPlanningServices>();


        public SeatPlanningServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }

        public async Task<Result<AddExamSessionResponse>> AddExamSession(AddExamSessionCommand addExamSessionCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

              

                    var addExamSession = new ExamSession(
                            newId,
                        addExamSessionCommand.name,
                        addExamSessionCommand.examDate,
                        schoolId ?? "",
                        true,
                             userId,
                        DateTime.UtcNow,
                        "",
                        default,
                  
                        addExamSessionCommand.ExamHallDTOs?.Select(e => new ExamHall(
                            Guid.NewGuid().ToString(),
                            e.hallName,
                            e.capacity,
                            newId
                        )).ToList() ?? new List<ExamHall>()

                    );

                    await _unitOfWork.BaseRepository<ExamSession>().AddAsync(addExamSession);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddExamSessionResponse>(addExamSession);
                    return Result<AddExamSessionResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Exam Result ", ex);

                }
            }
        }

        public async Task<Result<AddSeatPlannigResponse>> GenerateSeatPlanAsync(AddSeatPlanningCommand addSeatPlanningRequest)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var oldAssignments = await _unitOfWork.BaseRepository<SeatAssignment>()
                    .GetConditionalAsync(sa => sa.ExamSessionId == addSeatPlanningRequest.ExamSessionId);

                if (oldAssignments.Any())
                {
                    _unitOfWork.BaseRepository<SeatAssignment>().DeleteRange(oldAssignments.ToList());
                    await _unitOfWork.SaveChangesAsync();
                }

                var halls = await _unitOfWork.BaseRepository<ExamHall>()
                    .GetConditionalAsync(
                        eh => eh.ExamSessionId == addSeatPlanningRequest.ExamSessionId,
                        queryModifier: q => q.OrderBy(eh => eh.HallName)
                    );

                if (!halls.Any())
                    throw new Exception("No halls found for this exam session.");

                var students = await _unitOfWork.BaseRepository<StudentData>()
                    .GetConditionalFilterType(
                        s => addSeatPlanningRequest.classIds.Contains(s.ClassId),
                        q => q.Select(s => new StudentSelectDto(
                            s.Id,
                            s.FirstName + " " + s.LastName,
                            s.ClassId,
                            s.RegistrationNumber
                        ))
                    );

                var studentDetails = students.ToList();




                if (!studentDetails.Any())
                    throw new Exception("No students found for the given classIds.");

                ShuffleHelper.Shuffle(studentDetails);

                var assignments = new List<SeatAssignment>();
                var hallResponses = new List<HallSeatResponse>();

                int index = 0;

                foreach (var hall in halls)
                {
                    var hallStudents = new List<StudentSeatResponse>();

                    for (int seat = 1; seat <= hall.CapaCity; seat++)
                    {
                        if (index >= studentDetails.Count)
                            break;

                        var st = studentDetails[index];

                        assignments.Add(new SeatAssignment
                        {
                            Id = Guid.NewGuid().ToString(),
                            StudentId = st.Id,
                            ExamHallId = hall.Id,
                            ExamSessionId = addSeatPlanningRequest.ExamSessionId,
                            SeatNumber = seat
                        });

                        hallStudents.Add(new StudentSeatResponse(
                            st.Id,
                            st.FullName,
                            st.classId,
                            st.symbolNumber
                        ));

                        index++;
                    }

                    hallResponses.Add(new HallSeatResponse(
                        hall.Id,
                        hall.HallName,
                        hall.CapaCity,
                        hallStudents
                    ));

                    if (index >= studentDetails.Count)
                        break;
                }

                if (index < studentDetails.Count)
                    throw new Exception("Not enough hall capacity to allocate all students.");

                await _unitOfWork.BaseRepository<SeatAssignment>().AddRange(assignments);
                await _unitOfWork.SaveChangesAsync();

                var result = new AddSeatPlannigResponse(
                    addSeatPlanningRequest.ExamSessionId,
                    studentDetails.Count,
                    hallResponses
                );

                scope.Complete();
                return Result<AddSeatPlannigResponse>.Success(result);
            }
            catch (Exception ex)
            {
                scope.Dispose();
                throw new Exception("An error occurred while generating seat planning.", ex);
            }
        }

        public async Task<Result<PagedResult<ClassByExamSessionResponse>>> GetClassByExamSession(PaginationRequest paginationRequest, string examSessionId)
        {
            try
            {

                var (seatAssignments, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<SeatAssignment>();

                var seatAssignmentsDetails = seatAssignments.Where(x => x.ExamSessionId == examSessionId).AsNoTracking();

                if (!seatAssignmentsDetails.Any())
                    return Result<PagedResult<ClassByExamSessionResponse>>
                        .Failure("No classes assigned for this exam session.");

                var studentIds = seatAssignmentsDetails
                    .Select(sa => sa.StudentId)
                    .Distinct()
                    .ToList();

                var students = await _unitOfWork
                    .BaseRepository<StudentData>()
                    .GetConditionalAsync(s => studentIds.Contains(s.Id));

                var classIds = students
                   .Select(s => s.ClassId)
                   .Distinct()
                   .ToList();

                var classQuery = _unitOfWork
                    .BaseRepository<Class>()
                    .GetAsQueryable()
                    .Where(c => classIds.Contains(c.Id))
                    .AsNoTracking();


                var finalQuery = classQuery.AsQueryable().AsNoTracking();

                var pagedResult = await finalQuery.ToPagedResultAsync(
                      paginationRequest.pageIndex,
                      paginationRequest.pageSize,
                      paginationRequest.IsPagination);

                var mappedItems = new List<ClassByExamSessionResponse>
                    {
                        new ClassByExamSessionResponse(
                            examSessionId,
                            pagedResult.Data.Items.Select(x => x.Id).ToList()
                        )
                    };


                var response = new PagedResult<ClassByExamSessionResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<ClassByExamSessionResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Class", ex);
            }
        }

        public async Task<Result<PagedResult<FilterExamSessionResponse>>> GetFilterExamSession(PaginationRequest paginationRequest, FilterExamSessionDTOs filterExamSessionDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (examSession, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<ExamSession>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filterExamSession = isSuperAdmin
                    ? examSession
                    : examSession.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterExamSessionDTOs.startDate, filterExamSessionDTOs.endDate);

                var filteredResult = filterExamSession
                 .Where(x =>
                       (string.IsNullOrEmpty(filterExamSessionDTOs.name) || x.Name == filterExamSessionDTOs.name) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterExamSessionResponse(
                    i.Id,
                    i.Name,
                    i.Date,
                    i.SchoolId,
                    i.IsActive,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt
      

                ))
                .ToList();

                PagedResult<FilterExamSessionResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterExamSessionResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterExamSessionResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterExamSessionResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching subject: {ex.Message}", ex);
            }
        }
    }
}
