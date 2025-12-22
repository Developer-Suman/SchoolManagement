using AutoMapper;
using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.Academics.Command.AddSchoolClass;
using ES.Academics.Application.Academics.Command.UpdateExam;
using ES.Academics.Application.Academics.Command.UpdateSchoolClass;
using ES.Academics.Application.Academics.Queries.ClassByExamSession;
using ES.Academics.Application.Academics.Queries.Exam;
using ES.Academics.Application.Academics.Queries.ExamById;
using ES.Academics.Application.Academics.Queries.FilterExam;
using ES.Academics.Application.Academics.Queries.FilterSchoolClass;
using ES.Academics.Application.Academics.Queries.SchoolClass;
using ES.Academics.Application.Academics.Queries.SchoolClassById;
using ES.Academics.Application.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Account.Application.Account.Command.AddLedger;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace ES.Academics.Infrastructure.ServiceImpl
{
    public class ExamServices : IExamServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;

        public ExamServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddExamResponse>> Add(AddExamCommand addExamCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var addExam = new Exam(
                            newId,
                        addExamCommand.name,
                        addExamCommand.examDate,
                        FyId,
                        addExamCommand.isfinalExam,
                        schoolId ?? "",
                        userId,
                        DateTime.UtcNow,
                        "",
                        default,
                        true,
                        addExamCommand.classId
                    );

                    addExam.UpdateTotalMarks();

                    await _unitOfWork.BaseRepository<Exam>().AddAsync(addExam);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddExamResponse>(addExam);
                    return Result<AddExamResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Exam ", ex);

                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var exam = await _unitOfWork.BaseRepository<Exam>().GetByGuIdAsync(id);
                if (exam is null)
                {
                    return Result<bool>.Failure("NotFound", "exam Cannot be Found");
                }

                exam.IsActive = false;
                _unitOfWork.BaseRepository<Exam>().Update(exam);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting class having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<ExamQueryResponse>>> GetAllExam(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (exam, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<Exam>();

                var finalQuery = exam.Where(x=>x.IsActive == true && x.SchoolId == currentSchoolId).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<ExamQueryResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<ExamQueryResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<ExamQueryResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Class", ex);
            }

        }

       

        public async Task<Result<ExamByIdQueryResponse>> GetExam(string examId, CancellationToken cancellationToken = default)
        {
            try
            {
                var exam = await _unitOfWork.BaseRepository<Exam>().GetByGuIdAsync(examId);

                var examResponse = _mapper.Map<ExamByIdQueryResponse>(exam);

                return Result<ExamByIdQueryResponse>.Success(examResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Class by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<FilterExamResponse>>> GetFilterExam(PaginationRequest paginationRequest, FilterExamDTOs filterExamDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (exam, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Exam>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filterExam = isSuperAdmin
                    ? exam
                    : exam.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterExamDTOs.startDate, filterExamDTOs.endDate);

                var filteredResult = filterExam
                 .Where(x =>
                       (string.IsNullOrEmpty(filterExamDTOs.name) || x.Name == filterExamDTOs.name) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterExamResponse(
                
                    i.Id,
                    i.Name,
                    i.ExamDate,
                    i.TotalMarks,
         
                    i.IsFinalExam,
                    i.ClassId


                ))
                .ToList();

                PagedResult<FilterExamResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterExamResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterExamResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterExamResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching ledgers: {ex.Message}", ex);
            }
        }

        public async Task<Result<UpdateExamResponse>> Update(string examId, UpdateExamCommand updateExamCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (examId == null)
                    {
                        return Result<UpdateExamResponse>.Failure("NotFound", "Please provide valid examId");
                    }

                    var examToBeUpdated = await _unitOfWork.BaseRepository<Exam>().GetByGuIdAsync(examId);
                    if (examToBeUpdated is null)
                    {
                        return Result<UpdateExamResponse>.Failure("NotFound", "Exam are not Found");
                    }
                    examToBeUpdated.CreatedAt = DateTime.UtcNow;
                    _mapper.Map(updateExamCommand, examToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateExamResponse
                        (

                            examToBeUpdated.Name,
                            examToBeUpdated.ExamDate,
                    
                            examToBeUpdated.IsFinalExam,
                            examToBeUpdated.ClassId


                        );

                    return Result<UpdateExamResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the Class", ex);
                }
            }
        }
    }
}
