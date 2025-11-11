using AutoMapper;
using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.Academics.Command.AddExamResult;
using ES.Academics.Application.Academics.Command.UpdateExam;
using ES.Academics.Application.Academics.Command.UpdateExamResult;
using ES.Academics.Application.Academics.Queries.Exam;
using ES.Academics.Application.Academics.Queries.ExamById;
using ES.Academics.Application.Academics.Queries.ExamResult;
using ES.Academics.Application.Academics.Queries.ExamResultById;
using ES.Academics.Application.Academics.Queries.FilterExam;
using ES.Academics.Application.Academics.Queries.FilterExamResult;
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
    public class ExamResultServices : IExamResultServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;

        public ExamResultServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddExamResultResponse>> Add(AddExamResultCommand addExamResultCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var addExamResult = new ExamResult(
                            newId,
                        addExamResultCommand.examId,
                        addExamResultCommand.studentId,
                        addExamResultCommand.subjectId,
                        addExamResultCommand.marksObtained,
                        addExamResultCommand.grade,
                        addExamResultCommand.remarks,
                        true,
                        schoolId,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default
                        
                    );

                    await _unitOfWork.BaseRepository<ExamResult>().AddAsync(addExamResult);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddExamResultResponse>(addExamResult);
                    return Result<AddExamResultResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Exam Result ", ex);

                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var examResult = await _unitOfWork.BaseRepository<ExamResult>().GetByGuIdAsync(id);
                if (examResult is null)
                {
                    return Result<bool>.Failure("NotFound", "exam result Cannot be Found");
                }

                examResult.IsActive = false;
                _unitOfWork.BaseRepository<ExamResult>().Update(examResult);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting ExamResult having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<ExamResultResponse>>> GetAllExamResult(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (examResult, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<ExamResult>();

                var finalQuery = examResult.Where(x => x.IsActive == true).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<ExamResultResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<ExamResultResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<ExamResultResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all ExamResult", ex);
            }
        }

        public async Task<Result<ExamResultByIdResponse>> GetExamResult(string examResultId, CancellationToken cancellationToken = default)
        {
            try
            {
                var examResult = await _unitOfWork.BaseRepository<ExamResult>().GetByGuIdAsync(examResultId);

                var examResultResponse = _mapper.Map<ExamResultByIdResponse>(examResult);

                return Result<ExamResultByIdResponse>.Success(examResultResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Exam Result by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<FilterExamResultResponse>>> GetFilterExamResult(PaginationRequest paginationRequest, FilterExamResultDTOs filterExamResultDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (examResult, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<ExamResult>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filterExam = isSuperAdmin
                    ? examResult
                    : examResult.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterExamResultDTOs.startDate, filterExamResultDTOs.endDate);

                var filteredResult = filterExam
                 .Where(x =>
                       (string.IsNullOrEmpty(filterExamResultDTOs.studentId) || x.StudentId == filterExamResultDTOs.studentId) &&
                       (string.IsNullOrEmpty(filterExamResultDTOs.subjectId) || x.SubjectId == filterExamResultDTOs.subjectId) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterExamResultResponse(

                    i.ExamId,
                    i.StudentId,
                    i.SubjectId,
                    i.MarksObtained,
                    i.Grade,
                    i.Remarks,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


                ))
                .ToList();

                PagedResult<FilterExamResultResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterExamResultResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterExamResultResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterExamResultResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Exam Result: {ex.Message}", ex);
            }
        }

        public async Task<Result<UpdateExamResultResponse>> Update(string examResultId, UpdateExamResultCommand updateExamResultCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (examResultId == null)
                    {
                        return Result<UpdateExamResultResponse>.Failure("NotFound", "Please provide valid examResultId");
                    }

                    var examResultToBeUpdated = await _unitOfWork.BaseRepository<ExamResult>().GetByGuIdAsync(examResultId);
                    if (examResultToBeUpdated is null)
                    {
                        return Result<UpdateExamResultResponse>.Failure("NotFound", "Exam are not Found");
                    }
                    examResultToBeUpdated.CreatedAt = DateTime.UtcNow;
                    _mapper.Map(updateExamResultCommand, examResultToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateExamResultResponse
                        (

                            examResultToBeUpdated.ExamId,
                            examResultToBeUpdated.StudentId,
                            examResultToBeUpdated.SubjectId,
                            examResultToBeUpdated.MarksObtained,
                            examResultToBeUpdated.Grade,
                            examResultToBeUpdated.Remarks,
                            examResultToBeUpdated.IsActive,
                            examResultToBeUpdated.SchoolId,
                            examResultToBeUpdated.CreatedBy,
                            examResultToBeUpdated.CreatedAt,
                            examResultToBeUpdated.ModifiedBy,
                            examResultToBeUpdated.ModifiedAt



                        );

                    return Result<UpdateExamResultResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the Exam Result", ex);
                }
            }
        }
    }
}
