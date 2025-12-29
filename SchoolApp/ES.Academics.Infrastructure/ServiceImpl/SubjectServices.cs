using AutoMapper;
using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.Academics.Command.AddSubject;
using ES.Academics.Application.Academics.Command.UpdateExam;
using ES.Academics.Application.Academics.Command.UpdateSubject;
using ES.Academics.Application.Academics.Queries.Exam;
using ES.Academics.Application.Academics.Queries.ExamById;
using ES.Academics.Application.Academics.Queries.FilterExam;
using ES.Academics.Application.Academics.Queries.FilterSubject;
using ES.Academics.Application.Academics.Queries.Subject;
using ES.Academics.Application.Academics.Queries.SubjectById;
using ES.Academics.Application.ServiceInterface;
using Microsoft.EntityFrameworkCore;
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
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace ES.Academics.Infrastructure.ServiceImpl
{
    public class SubjectServices : ISubjectServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;


        public SubjectServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddSubjectResponse>> Add(AddSubjectCommand addSubjectCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    var exam = await _unitOfWork.BaseRepository<Exam>().GetByGuIdAsync(addSubjectCommand.examId);
                    if (exam == null) return Result<AddSubjectResponse>.Failure("The specified Exam does not exist.");

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var addSubject = new Subject(
                            newId,
                        addSubjectCommand.name,
                        addSubjectCommand.code,
                        addSubjectCommand.creditHours,
                        addSubjectCommand.description,
                        addSubjectCommand.classId,
                
                        schoolId ?? "",
                        true,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default,
                        addSubjectCommand.fullMarks,
                        addSubjectCommand.passMarks,
                        addSubjectCommand.examId
                    );

                    await _unitOfWork.BaseRepository<Subject>().AddAsync(addSubject);

                    //Update the TotalMarks on the Parent Exam, sum existing and new one

                    var allSubjectsForThisExam = await _unitOfWork.BaseRepository<Subject>()
                    .GetConditionalAsync(x => x.ExamId == addSubjectCommand.examId);

                    exam.TotalMarks = allSubjectsForThisExam.Sum(s => s.FullMarks) + addSubject.FullMarks;

                     _unitOfWork.BaseRepository<Exam>().Update(exam);



                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddSubjectResponse>(addSubject);
                    return Result<AddSubjectResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Subject ", ex);

                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var subject = await _unitOfWork.BaseRepository<Subject>().GetByGuIdAsync(id);
                if (subject is null)
                {
                    return Result<bool>.Failure("NotFound", "subject Cannot be Found");
                }

                subject.IsActive = false;
                _unitOfWork.BaseRepository<Subject>().Update(subject);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting class having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<SubjectResponse>>> GetAllSubject(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (subject, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<Subject>();

                var finalQuery = subject.Where(x => x.IsActive == true).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<SubjectResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<SubjectResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<SubjectResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Class", ex);
            }
        }

        public async Task<Result<PagedResult<FilterSubjectResponse>>> GetFilterSubject(PaginationRequest paginationRequest, FilterSubjectDTOs filterSubjectDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (subject, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Subject>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filterExam = isSuperAdmin
                    ? subject
                    : subject.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterSubjectDTOs.startDate, filterSubjectDTOs.endDate);

                var filteredResult = filterExam
                 .Where(x =>
                       (string.IsNullOrEmpty(filterSubjectDTOs.name) || x.Name == filterSubjectDTOs.name) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterSubjectResponse(
                    i.Id,
                    i.Name,
                    i.Code,
                    i.CreditHours,
                    i.Description,
                    i.ClassId,
                    i.SchoolId,
                    i.IsActive,
                    i.FullMarks,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


                ))
                .ToList();

                PagedResult<FilterSubjectResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterSubjectResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterSubjectResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterSubjectResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching subject: {ex.Message}", ex);
            }
        }

        public async Task<Result<SubjectByIdResponse>> GetSubject(string subjectId, CancellationToken cancellationToken = default)
        {
            try
            {
                var subject = await _unitOfWork.BaseRepository<Subject>().GetByGuIdAsync(subjectId);

                var subjectResponse = _mapper.Map<SubjectByIdResponse>(subject);

                return Result<SubjectByIdResponse>.Success(subjectResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Class by using Id", ex);
            }
        }

        public async Task<Result<UpdateSubjectResponse>> Update(string subjectId, UpdateSubjectCommand updateSubjectCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (subjectId == null)
                    {
                        return Result<UpdateSubjectResponse>.Failure("NotFound", "Please provide valid subjectId");
                    }

                    var subjectToBeUpdated = await _unitOfWork.BaseRepository<Subject>().GetByGuIdAsync(subjectId);
                    if (subjectToBeUpdated is null)
                    {
                        return Result<UpdateSubjectResponse>.Failure("NotFound", "Subject are not Found");
                    }
                    subjectToBeUpdated.CreatedAt = DateTime.UtcNow;
                    _mapper.Map(updateSubjectCommand, subjectToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateSubjectResponse
                        (

                            subjectToBeUpdated.Name,
                            subjectToBeUpdated.Code,
                            subjectToBeUpdated.CreditHours,
                            subjectToBeUpdated.Description,
                            subjectToBeUpdated.ClassId,
                            subjectToBeUpdated.SchoolId,
                            subjectToBeUpdated.IsActive,
                            subjectToBeUpdated.CreatedBy,
                            subjectToBeUpdated.CreatedAt,
                            subjectToBeUpdated.ModifiedBy,
                            subjectToBeUpdated.ModifiedAt


                        );

                    return Result<UpdateSubjectResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the Subject", ex);
                }
            }
        }
    }
}
