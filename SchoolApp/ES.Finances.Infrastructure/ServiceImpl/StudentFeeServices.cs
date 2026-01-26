using AutoMapper;
using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure;
using ES.Finances.Application.Finance.Command.Fee.AddStudentFee;
using ES.Finances.Application.Finance.Command.Fee.AssignMonthlyFee;
using ES.Finances.Application.Finance.Command.Fee.UpdateStudentFee;
using ES.Finances.Application.Finance.Command.PaymentRecords.AddpaymentsRecords;
using ES.Finances.Application.Finance.Queries.Fee.Feetype;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeetype;
using ES.Finances.Application.Finance.Queries.Fee.FilterStudentFee;
using ES.Finances.Application.Finance.Queries.Fee.StudentFee;
using ES.Finances.Application.Finance.Queries.Fee.StudentFeeById;
using ES.Finances.Application.Finance.Queries.Fee.StudentFeeSummary;
using ES.Finances.Application.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Account.Application.Account.Command.AddJournalEntry;
using TN.Account.Application.Account.Command.AddJournalEntryDetails;
using TN.Account.Application.Account.Command.AddLedger;
using TN.Account.Application.ServiceInterface;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Communication;
using TN.Shared.Domain.Entities.Finance;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.SchoolItems;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using static TN.Shared.Domain.Entities.Finance.StudentFee;

namespace ES.Finances.Infrastructure.ServiceImpl
{
    public class StudentFeeServices : IStudentFeeServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        private readonly ILedgerService _ledgerService;
        private readonly IJournalServices _journalServices;

        public StudentFeeServices(ILedgerService  ledgerService,IJournalServices journalServices,IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
            _ledgerService = ledgerService;
            _journalServices = journalServices;
        }
        public async Task<Result<AddStudentFeeResponse>> Add(AddStudentFeeCommand addStudentFeeCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? throw new Exception("School ID not found.");
                    var userId = _tokenService.GetUserId();

                    var feeStructure = await _unitOfWork.BaseRepository<FeeStructure>()
                        .GetByGuIdAsync(addStudentFeeCommand.feeStructureId);

                    if (feeStructure == null)
                        throw new Exception("Fee structure not found.");

                    var studentFee = await _unitOfWork.BaseRepository<StudentFee>()
                        .FirstOrDefaultAsync(x => x.StudentId == addStudentFeeCommand.studentId &&
                                                  x.ClassId == addStudentFeeCommand.classId);

                    decimal currentTotal = studentFee?.TotalAmount ?? feeStructure.Amount;

                    decimal discountAmount = feeStructure.Amount * (addStudentFeeCommand.discountPercentage / 100);

                    decimal tobePaid = currentTotal - discountAmount;




                    var newRecord = new StudentFee(
                        Guid.NewGuid().ToString(),
                        addStudentFeeCommand.studentId,
                        addStudentFeeCommand.feeStructureId,
                        addStudentFeeCommand.classId,
                        discountAmount,
                        addStudentFeeCommand.discountPercentage,
                        
                        currentTotal,
                        tobePaid,
                        true,
                        schoolId,
                        PaidStatus.Pending,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default
                    );

                    await _unitOfWork.BaseRepository<StudentFee>().AddAsync(newRecord);

                    var students = await _unitOfWork.BaseRepository<StudentData>()
                       .GetByGuIdAsync(addStudentFeeCommand.studentId);

                    #region Tax part
                    //var taxAmount = (13 / 100) * currentPaid;

                    //var totalPayable = currentPaid + taxAmount - discountAmount;
                    #endregion

                    #region Journal Entries
                    var newJournalId = Guid.NewGuid().ToString();
                    var netAmountDue = currentTotal - discountAmount;

                    var journalDetails = new List<JournalEntryDetails>();
                    journalDetails.Add(new JournalEntryDetails(
                                        Guid.NewGuid().ToString(),
                                        newJournalId,
                                        students.LedgerId,
                                        netAmountDue,
                                        0,
                                        DateTime.UtcNow,
                                        schoolId,
                                        FyId,
                                        true
                                    ));

                    // We record the full income so your reports show the total potential revenue.
                    journalDetails.Add(new JournalEntryDetails(
                        Guid.NewGuid().ToString(),
                        newJournalId,
                        feeStructure.LedgerId,
                        0,
                        currentTotal, // Credit the full original amount
                        DateTime.UtcNow,
                        schoolId,
                        FyId,
                        true
                    ));

                    if (discountAmount > 0)
                    {
                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            LedgerConstants.DiscountLedgerId,
                            discountAmount, // Debit the discount as an expense
                            0,
                            DateTime.UtcNow,
                            schoolId,
                            FyId,
                            true
                        ));
                    }


                    //if (taxAmount > 0)
                    //{
                    //    journalDetails.Add(new JournalEntryDetails(
                    //        Guid.NewGuid().ToString(),
                    //        newJournalId,
                    //        LedgerConstants.VATLedgerId,     
                    //        0,
                    //        taxAmount,       
                    //        DateTime.UtcNow,
                    //        schoolId,
                    //        FyId,
                    //        true
                    //    ));
                    //}




                    var journalData = new JournalEntry(
                           newJournalId,
                           "Student Fee Assigned Voucher",
                           DateTime.UtcNow,
                           "Being Students fees Assigned",
                           userId,
                           schoolId,
                           DateTime.UtcNow,
                           "",
                           default,
                           "",
                           FyId,
                           true,
                           journalDetails
                       );

                    decimal totalDebitFinal = journalDetails.Sum(x => x.DebitAmount);
                    decimal totalCreditFinal = journalDetails.Sum(x => x.CreditAmount);

                    await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);


                    #endregion









                    await _unitOfWork.SaveChangesAsync();

                    // 4. Commit transaction
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddStudentFeeResponse>(newRecord);
                    return Result<AddStudentFeeResponse>.Success(resultDTOs);
                }
                catch (Exception ex)
                {
                    // Log ex here
                    throw new Exception("An error occurred while processing the student fee.", ex);
                }
            }
        }

        public async Task<Result<AssignMonthlyFeeResponse>> AssignMonthlyFee(AssignMonthlyFeeCommand assignMonthlyFeeCommand)
        {
            try
            {
                string newId = Guid.NewGuid().ToString();
                var FyId = _fiscalContext.CurrentFiscalYearId;
                var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                var userId = _tokenService.GetUserId();


                var feeStructure = await _unitOfWork.BaseRepository<FeeStructure>()
                    .FirstOrDefaultAsync(x =>
                        x.FeeTypeId == assignMonthlyFeeCommand.feeTypeId &&
                        x.ClassId == assignMonthlyFeeCommand.classId &&
                        x.IsActive);

                if (feeStructure == null)
                    throw new Exception("Fee structure not defined for this class.");

                var students = (await _unitOfWork.BaseRepository<StudentData>()
                      .FindBy(x =>
                          x.ClassId == assignMonthlyFeeCommand.classId &&
                          x.IsActive))
                      .ToList();

                if (!students.Any())
                    return Result<AssignMonthlyFeeResponse>.Failure(
                        "No active students found for this class.");

                var feetypes = (await _unitOfWork.BaseRepository<FeeType>()
                      .GetByGuIdAsync(assignMonthlyFeeCommand.feeTypeId));

                var studentIds = students.Select(s => s.Id).ToList();

                foreach (var student in students)
                {

                    var addJournal = new AddJournalEntryCommand(
                        $"Opening balance for {student.FirstName} {student.LastName}",
                        DateTime.UtcNow.ToString(),
                        "Being opening balance entry",
                        new List<AddJournalEntryDetailsRequest>
                        {
                                new AddJournalEntryDetailsRequest(
                                    student.LedgerId,
                                    feeStructure.Amount,
                                    0
                                ),
                                new AddJournalEntryDetailsRequest(
                                    feeStructure.LedgerId,
                                    0,
                                    feeStructure.Amount
                                )
                                    }
                                );

                    await _journalServices.Add(addJournal);
                }

                await _unitOfWork.SaveChangesAsync();

                var response = new AssignMonthlyFeeResponse(
                    assignMonthlyFeeCommand.classId,
                    assignMonthlyFeeCommand.feeTypeId);

                return Result<AssignMonthlyFeeResponse>.Success(response);


            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while assigning monthly fee", ex);
            }
        }

        public async Task<Result<PagedResult<FilterStudentFeeResponse>>> Filter(PaginationRequest paginationRequest, FilterStudentFeeDTOs filterStudentFeeDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (studentFee, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<StudentFee>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
                    ? studentFee
                    : studentFee.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterStudentFeeDTOs.startDate, filterStudentFeeDTOs.endDate);

                var filteredResult = filter
                  .Where(x =>
                      (string.IsNullOrEmpty(filterStudentFeeDTOs.studentId) || x.StudentId == filterStudentFeeDTOs.studentId) &&
                      x.CreatedAt >= startUtc &&
                      x.CreatedAt <= endUtc &&
                      x.IsActive
                  )
                  .GroupBy(x => new { x.StudentId, x.ClassId })
                  .Select(g => new {
                      StudentId = g.Key.StudentId,
                      FeeStructureIds = g.Select(x => $"{x.FeeStructure.FeeType.Name} -> {x.DiscountPercentage}%").ToList(),
                      TotalAmount = g.Sum(x => x.TotalAmount),
                      TotalPaid = g.Sum(x => x.PaidAmount),

                      // Calculation (must use g.Sum again here or do it in the next Select)
                      DueAmount = g.Sum(x => x.TotalAmount) - g.Sum(x => x.PaidAmount),

                      LatestDate = g.Max(x => x.CreatedAt),
                      ClassId = g.Select(x=>x.ClassId)
                  })
                  .OrderByDescending(x => x.LatestDate)
                  .ToList();

                var responseList = filteredResult
                    .Select(i => new FilterStudentFeeResponse(
                        i.StudentId,
                        i.FeeStructureIds, 
                        i.TotalAmount,
                        i.TotalPaid,
                        i.DueAmount,
                        i.ClassId.FirstOrDefault()
                    ))
                    .ToList();

                PagedResult<FilterStudentFeeResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterStudentFeeResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterStudentFeeResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterStudentFeeResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching result: {ex.Message}", ex);
            }
        }

        public async Task<Result<StudentFeeByIdResponse>> GetStudentFee(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var studentFee = await _unitOfWork.BaseRepository<Notice>().GetByGuIdAsync(id);

                var studentFeeResponse = _mapper.Map<StudentFeeByIdResponse>(studentFee);

                return Result<StudentFeeByIdResponse>.Success(studentFeeResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Notice by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<StudentFeeSummaryResponse>>> GetStudentFeeSummary(PaginationRequest paginationRequest, StudentFeeSummaryDTOs studentFeeSummaryDTOs)
        {
            try
            {
                string schoolId = _tokenService.SchoolId().FirstOrDefault();

                var query = _unitOfWork.BaseRepository<PaymentsRecords>()
                .GetAllWithIncludeQueryable(x =>
                    x.Schoolid == schoolId &&
                    x.StudentId == studentFeeSummaryDTOs.studentId
                )

                .Include(x => x.StudentFee)
                .AsNoTracking();

                // 2. Count Total Records at the Database level
                int totalItems = await query.CountAsync();

                // 3. Apply Pagination and Projection
                int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                List<StudentFeeSummaryResponse> items;

                //decimal totalRemaining = await _unitOfWork.BaseRepository<StudentFee>()
                //      .GetAsQueryable()
                //      .Where(x => x.StudentId == studentFeeSummaryDTOs.studentId)
                //      .SumAsync(x => x.TotalAmount - x.PaidAmount - x.DiscountAmount);

                if (paginationRequest.IsPagination)
                {
                    items = await query
                        .OrderByDescending(x => x.CreatedAt) // Always sort when paginating!
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .Select(x => new StudentFeeSummaryResponse(
                            x.StudentFee.ClassId,
                            x.StudentFee.PaidAmount,
                            x.PaymentMethod,
                            x.StudentFee.TotalAmount,
                            x.StudentFee.TotalAmount - x.StudentFee.PaidAmount
                        ))
                        .ToListAsync();
                }
                else
                {
                    items = await query
                        .Select(x => new StudentFeeSummaryResponse(
                            x.StudentFee.ClassId,
                            x.AmountPaid,
                            x.PaymentMethod,
                            x.StudentFee.TotalAmount,
                             x.StudentFee.TotalAmount - x.StudentFee.PaidAmount
                        ))
                        .ToListAsync();
                    pageSize = items.Count;
                }

                var finalResponseList = new PagedResult<StudentFeeSummaryResponse>
                {
                    Items = items,
                    TotalItems = totalItems,
                    PageIndex = pageIndex,
                    pageSize = pageSize
                };

                return Result<PagedResult<StudentFeeSummaryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                // Avoid throwing raw exceptions; return a Failure result for better API handling
                return Result<PagedResult<StudentFeeSummaryResponse>>.Failure($"Error: {ex.Message}");
            }
        }

        public async Task<Result<PagedResult<StudentFeeResponse>>> StudentFee(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (studentFee, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<StudentFee>();

                var finalQuery = studentFee.Where(x => x.IsActive == true && x.SchoolId == currentSchoolId).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<StudentFeeResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<StudentFeeResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<StudentFeeResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
        }

        public async Task<Result<UpdateStudentFeeResponse>> Update(string studentFeeId, UpdateStudentFeeCommand updateStudentFeeCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (studentFeeId == null)
                    {
                        return Result<UpdateStudentFeeResponse>.Failure("NotFound", "Please provide valid studentFeeId");
                    }

                    var studentFeeToBeUpdated = await _unitOfWork.BaseRepository<StudentFee>().GetByGuIdAsync(studentFeeId);
                    if (studentFeeToBeUpdated is null)
                    {
                        return Result<UpdateStudentFeeResponse>.Failure("NotFound", "StudentFee not Found");
                    }
                    studentFeeToBeUpdated.ModifiedAt = DateTime.UtcNow;
                    _mapper.Map(updateStudentFeeCommand, studentFeeToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateStudentFeeResponse
                        (
                            studentFeeId,
                            studentFeeToBeUpdated.StudentId,
                            studentFeeToBeUpdated.FeeStructureId,
                            studentFeeToBeUpdated.DiscountAmount,
                            studentFeeToBeUpdated.TotalAmount,
                            studentFeeToBeUpdated.PaidAmount,
                            studentFeeToBeUpdated.IsActive,
                            studentFeeToBeUpdated.SchoolId,
                            studentFeeToBeUpdated.CreatedBy,
                            studentFeeToBeUpdated.CreatedAt,
                            studentFeeToBeUpdated.ModifiedBy,
                            studentFeeToBeUpdated.ModifiedAt
                        );

                    return Result<UpdateStudentFeeResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating StuentFee", ex);
                }
            }
        }
    }
}
