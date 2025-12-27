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
using ES.Academics.Application.Academics.Queries.MarkSheetByStudent;
using ES.Academics.Application.Academics.Queries.SubjectByClassId;
using ES.Academics.Application.ServiceInterface;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Account.Application.Account.Command.UpdateJournalEntry;
using TN.Account.Application.Account.Queries.FilterJournalByDate;
using TN.Account.Application.Account.Queries.JournalEntry;
using TN.Account.Application.Account.Queries.JournalEntryById;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Authentication.Domain.Static.Roles;
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
        private readonly IHelperMethodServices _helperMethodServices;

        public ExamResultServices(IDateConvertHelper dateConverter, IHelperMethodServices helperMethodServices,IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _helperMethodServices = helperMethodServices;
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

                    var checkDoubleEntryDetails = await _unitOfWork.BaseRepository<ExamResult>()
                        .FirstOrDefaultAsync(x => x.StudentId == addExamResultCommand.studentId && x.ExamId == addExamResultCommand.examId);

                    if(checkDoubleEntryDetails != null)
                    {
                        return Result<AddExamResultResponse>.Failure("ForbiddenAccess", "Details already in the system");
                    }

                    var addExamResult = new ExamResult(
                            newId,
                        addExamResultCommand.examId,
                        addExamResultCommand.studentId,
                        addExamResultCommand.remarks,
                        true,
                        schoolId,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default,
                        _fiscalContext.CurrentFiscalYearId,
                        addExamResultCommand.marksObtained?.Select(e=> new MarksObtained(
                            Guid.NewGuid().ToString(),
                            
                            e.subjectId,
                            e.marksObtained,
                            newId
                        )).ToList() ?? new List<MarksObtained>()

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

                var FyId = _fiscalContext.CurrentFiscalYearId;
                var schoolId = _tokenService.SchoolId().FirstOrDefault();
                var institutionId = _tokenService.InstitutionId() ?? string.Empty;
                var isSuperAdmin = _tokenService.GetRole() == Role.SuperAdmin;

                IEnumerable<ExamResult> examResults;
                if (isSuperAdmin)
                {
                    examResults = await _unitOfWork.BaseRepository<ExamResult>()
                        .GetConditionalAsync(
                            x => x.FyId == FyId && x.IsActive,
                            query => query.Include(rm => rm.MarksOtaineds));
                }
                else if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(schoolId))
                {
                    var schoolIds = await _unitOfWork.BaseRepository<School>()
                        .GetConditionalFilterType(
                            x => x.InstitutionId == institutionId,
                            query => query.Select(c => c.Id));

                    examResults = await _unitOfWork.BaseRepository<ExamResult>()
                        .GetConditionalAsync(
                            x => x.FyId == FyId && schoolIds.Contains(x.SchoolId) && x.IsActive,
                            query => query.Include(j => j.MarksOtaineds));
                }
                else
                {
                    examResults = await _unitOfWork.BaseRepository<ExamResult>()
                        .GetConditionalAsync(
                            x => x.FyId == FyId && x.SchoolId == schoolId && x.IsActive,
                            query => query.Include(j => j.MarksOtaineds));
                }


                var examResultResponse = examResults
                       .OrderByDescending(examResult => examResult.CreatedAt) // Recent first
                       .Select(exam => new ExamResultResponse(
                           exam.Id,
                           exam.ExamId,
                           exam.StudentId,
                           exam.Remarks,
                           exam.IsActive,
                           exam.SchoolId,
                           exam.CreatedBy,
                           exam.CreatedAt,
                           exam.ModifiedBy,
                           exam.ModifiedAt,
                           exam.MarksOtaineds?.Select(detail => new MarksObtainedDTOs(
                               detail.SubjectId,
                               detail.MarksObtaineds
                      
                              
                           )).ToList() ?? new List<MarksObtainedDTOs>()
                       ))
                       .ToList();

                var totalItems = examResultResponse.Count;

                var paginatedJournalEntries = paginationRequest != null && paginationRequest.IsPagination
                    ? examResultResponse
                        .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                        .Take(paginationRequest.pageSize)
                        .ToList()
                    : examResultResponse;

                var pagedResult = new PagedResult<ExamResultResponse>
                {
                    Items = paginatedJournalEntries,
                    TotalItems = totalItems,
                    PageIndex = paginationRequest?.pageIndex ?? 1,
                    pageSize = paginationRequest?.pageSize ?? totalItems
                };


                return Result<PagedResult<ExamResultResponse>>.Success(pagedResult);
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
                var examResults = await _unitOfWork.BaseRepository<ExamResult>().
                    GetConditionalAsync(x => x.Id == examResultId,
                    query => query.Include(rm => rm.MarksOtaineds)
                    );

                var exam = examResults.FirstOrDefault();
                var examResult = new ExamResultByIdResponse(
                    exam.Id,
                    exam.ExamId,
                    exam.StudentId,
                    exam.Remarks,
                    exam.IsActive,
                    exam.SchoolId,
                    exam.CreatedBy,
                    exam.CreatedAt,
                    exam.ModifiedBy,
                    exam.ModifiedAt,
                    exam.MarksOtaineds?.Select(detail => new MarksObtainedDTOs(
                        detail.SubjectId,
                        detail.MarksObtaineds
                       
                    )).ToList() ?? new List<MarksObtainedDTOs>()
                );

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
                var fiscalYearId = _fiscalContext.CurrentFiscalYearId;
                var schoolId = _tokenService.SchoolId().FirstOrDefault();
                var institutionId = _tokenService.InstitutionId() ?? string.Empty;
                var isSuperAdmin = _tokenService.GetRole() == Role.SuperAdmin;
                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterExamResultDTOs.startDate, filterExamResultDTOs.endDate);


                IEnumerable<ExamResult> examResults;

                if (isSuperAdmin)
                {
                    examResults = await _unitOfWork.BaseRepository<ExamResult>().GetConditionalAsync(
                        x =>
                            x.FyId == fiscalYearId && x.IsActive &&
                            x.CreatedAt >= startUtc &&
                            x.CreatedAt <= endUtc &&
                            (string.IsNullOrEmpty(filterExamResultDTOs.studentId) || x.StudentId.Contains(filterExamResultDTOs.studentId)),
                        q => q.Include(x => x.MarksOtaineds)
                    );
                }
                else if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(schoolId))
                {
                    var schoolIds = await _unitOfWork.BaseRepository<School>()
                        .GetConditionalFilterType(
                            x => x.InstitutionId == institutionId,
                            q => q.Select(c => c.Id));

                    examResults = await _unitOfWork.BaseRepository<ExamResult>().GetConditionalAsync(
                        x =>
                            x.FyId == fiscalYearId &&
                            schoolIds.Contains(x.SchoolId) && x.IsActive &&
                            x.CreatedAt >= startUtc &&
                            x.CreatedAt <= endUtc &&
                             (string.IsNullOrEmpty(filterExamResultDTOs.studentId) || x.StudentId.Contains(filterExamResultDTOs.studentId)),
                        q => q.Include(x => x.MarksOtaineds)
                    );
                }
                else
                {
                    examResults = await _unitOfWork.BaseRepository<ExamResult>().GetConditionalAsync(
                        x =>
                            x.FyId == fiscalYearId && x.IsActive &&
                            x.SchoolId == schoolId &&
                            x.CreatedAt >= startUtc &&
                            x.CreatedAt <= endUtc &&
                            (string.IsNullOrEmpty(filterExamResultDTOs.studentId) || x.StudentId.Contains(filterExamResultDTOs.studentId)),
                        q => q.Include(x => x.MarksOtaineds)
                    );
                }





                var responseList = examResults
                .OrderByDescending(x => x.CreatedAt)
                .Select(exam => new FilterExamResultResponse(
                    exam.Id,
                    exam.ExamId,
                    exam.StudentId,
                    exam.Remarks,
                    exam.IsActive,
                    exam.SchoolId,
                    exam.CreatedBy,
                    exam.CreatedAt,
                    exam.ModifiedBy,
                    exam.ModifiedAt,
                    exam.MarksOtaineds?.Select(detail => new MarksObtainedDTOs(
                        detail.SubjectId,
                        detail.MarksObtaineds
                     
                    )).ToList() ?? new List<MarksObtainedDTOs>()


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

        public async Task<Result<MarkSheetByStudentResponse>> GetMarkSheet(MarksSheetDTOs marksSheetDTOs, CancellationToken cancellationToken = default)
        {
            try
            {
                var marksSheetResult = await _unitOfWork.BaseRepository<ExamResult>().
                    GetConditionalAsync(x => x.StudentId == marksSheetDTOs.studentId && x.ExamId == marksSheetDTOs.examId,
                    query => query.Include(rm => rm.MarksOtaineds)
                    );

                var marksSheet = marksSheetResult.FirstOrDefault();

                var percentage =await _helperMethodServices.CalculatePercentage(marksSheetDTOs);

                decimal percentageValue = 0;

                if (!string.IsNullOrWhiteSpace(percentage))
                {
                    percentageValue = decimal.Parse(percentage.Replace("%", ""));
                }


                var totalObtainedMarks = marksSheet.MarksOtaineds.Sum(x => x.MarksObtaineds);

                var grade = await _helperMethodServices.CalculateGPA(marksSheetDTOs);

                var division = await _helperMethodServices.CalculateDivision(marksSheetDTOs);

                if (marksSheet == null)
                {
                    return Result<MarkSheetByStudentResponse>.Failure("NotFound", "Marksheet doesnot find from this query");
                }

                var marksSheetDetails = new MarkSheetByStudentResponse(
                     marksSheet?.ExamId ?? string.Empty,
                     marksSheet?.StudentId ?? string.Empty,
                     marksSheet?.Remarks ?? string.Empty,
                     marksSheet?.IsActive ?? false,
                     marksSheet?.SchoolId ?? string.Empty,
                     marksSheet?.CreatedBy ?? string.Empty,
                     marksSheet?.CreatedAt ?? default,
                     marksSheet?.ModifiedBy ?? string.Empty,
                     marksSheet?.ModifiedAt ?? default,
                     percentage,
                     totalObtainedMarks,
                     GetGrade(percentageValue),
                     GetGpa(percentageValue),
                     division,
                     marksSheet?.MarksOtaineds?
                         .Select(detail =>
                         {
                             var subjectPercentage = totalObtainedMarks > 0
                                 ? (detail.MarksObtaineds / totalObtainedMarks) * 100
                                 : 0;

                             return new MarksWithGrades(
                                 detail.SubjectId ?? string.Empty,
                                 detail.MarksObtaineds,
                                 GetGrade(subjectPercentage),
                                 GetGpa(subjectPercentage)
                             );
                         })
                         .ToList()
                     ?? new List<MarksWithGrades>()
                 );



                var marksSheetResponse = _mapper.Map<MarkSheetByStudentResponse>(marksSheetDetails);

                return Result<MarkSheetByStudentResponse>.Success(marksSheetResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the marksheet", ex);
            }
        }

        private string GetGrade(decimal percentage)
        {
            if (percentage >= 90) return "A+";
            if (percentage >= 80) return "A";
            if (percentage >= 70) return "B+";
            if (percentage >= 60) return "B";
            if (percentage >= 50) return "C";
            if (percentage >= 40) return "D";
            return "E";
        }

        private decimal GetGpa(decimal percentage)
        {
            if (percentage >= 90) return 4.0m;
            if (percentage >= 80) return 3.6m;
            if (percentage >= 70) return 3.2m;
            if (percentage >= 60) return 2.8m;
            if (percentage >= 50) return 2.4m;
            if (percentage >= 40) return 2.0m;

            return 0.8m;
        }

        private decimal Percentage(decimal obtained, decimal total)
        {
            if (total <= 0) return 0;
            return (obtained / total) * 100;
        }




        public async Task<Result<List<SubjectByClassIdResponse>>> GetSubjectByClass(string classId, CancellationToken cancellationToken = default)
        {
            try
            {

                var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";

                var allSubject = await _unitOfWork.BaseRepository<Subject>().
                    GetConditionalAsync(x => x.ClassId == classId && x.SchoolId == schoolId
                    );

                var subjectResponse = allSubject
                    .Select(subject => new SubjectByClassIdResponse(
                        subject.Id,
                        subject.Name,
                        subject.FullMarks
                    ))
                    .ToList();

                return Result<List<SubjectByClassIdResponse>>.Success(subjectResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching AllSubject Result by using ClassId", ex);
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
                    var userId = _tokenService.GetUserId();
                    var examResultsDetails = await _unitOfWork.BaseRepository<ExamResult>().
                                 GetConditionalAsync(x => x.Id == examResultId,
                                 query => query.Include(rm => rm.MarksOtaineds)
                                 );

                    var examResult = examResultsDetails.FirstOrDefault();

                    if (examResult == null)
                    {
                        return Result<UpdateExamResultResponse>.Failure("NotFound", "ExamResult not found.");
                    }


                    examResult.ModifiedBy = userId;
                    examResult.ModifiedAt = DateTime.UtcNow;
                    examResult.Remarks = updateExamResultCommand.remarks;
                    examResult.ExamId = updateExamResultCommand.examId;
                    examResult.StudentId = updateExamResultCommand.studentId;




                    if (updateExamResultCommand.marksObtained != null && updateExamResultCommand.marksObtained.Any())
                    {
                        foreach (var detail in updateExamResultCommand.marksObtained)
                        {
                            var existingExamResult = await _unitOfWork.BaseRepository<ExamResult>().GetByGuIdAsync(detail.Id);

                            if (existingExamResult != null)
                            {
                                _mapper.Map(detail, existingExamResult);
                                _unitOfWork.BaseRepository<ExamResult>().Update(existingExamResult);
                            }
                            else
                            {
                                var newExamResult = _mapper.Map<ExamResult>(detail);
                                newExamResult.Id = Guid.NewGuid().ToString();
                                await _unitOfWork.BaseRepository<ExamResult>().AddAsync(newExamResult);
                            }
                        }

                        await _unitOfWork.SaveChangesAsync();
                    }

                    _mapper.Map(updateExamResultCommand, examResult);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();


                    var resultResponse = new UpdateExamResultResponse
                        (

                            examResult.ExamId,
                            examResult.StudentId,

                            examResult.Remarks,
                            examResult.IsActive,
                            examResult.SchoolId,
                            examResult.CreatedBy,
                            examResult.CreatedAt,
                            examResult.ModifiedBy,
                            examResult.ModifiedAt,
                            examResult.MarksOtaineds?.Select(details=> new MarksObtainedDTOs
                            (
                                details.SubjectId,
                                details.MarksObtaineds
                          
                            )).ToList() ?? new List<MarksObtainedDTOs>()



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
