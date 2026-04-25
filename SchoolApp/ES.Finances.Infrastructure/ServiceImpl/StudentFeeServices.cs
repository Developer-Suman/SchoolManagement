using AutoMapper;
using Azure.Core;
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
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Communication;
using TN.Shared.Domain.Entities.Finance;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.SchoolItems;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using static TN.Shared.Domain.Entities.Finance.StudentFee;
using static TN.Shared.Domain.Enum.HelperEnum;

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
                    var newStudentFeeId = Guid.NewGuid().ToString();

                    // 1. Fetch Structure and specific Student's existing Fees in one trip
                    var feeStructureList = await _unitOfWork.BaseRepository<FeeStructure>()
                            .GetConditionalAsync(x => x.Id == addStudentFeeCommand.feeStructureId
                            && x.ClassId == addStudentFeeCommand.classId,
                            query => query
                                .Include(x => x.FeeStructureDetails)
                                .Include(x => x.StudentFees.Where(sf => sf.StudentId == addStudentFeeCommand.studentId))
                                .ThenInclude(sf => sf.StudentFeeDetails)
                            );

                    var entity = feeStructureList.FirstOrDefault();
                    if (entity == null)
                        return Result<AddStudentFeeResponse>.Failure("NotFound", "Fee structure not found for this class.");

                    // 2. Map existing data for fast lookup
                    var existingTemplateTypeIds = entity.FeeStructureDetails.Select(fd => fd.FeeTypeId).ToHashSet();
                    var existingStudentTypeIds = entity.StudentFees
                        .SelectMany(sf => sf.StudentFeeDetails)
                        .Select(sfd => sfd.FeeTypeId)
                        .ToHashSet();

                    // 3. Filter for truly NEW items only
                    var itemsToAssign = addStudentFeeCommand.StudentFeeDetailsDTOs
                        .Where(dto => dto != null && !existingStudentTypeIds.Contains(dto.feeTypeId))
                        .ToList();

                    if (!itemsToAssign.Any())
                    {
                        return Result<AddStudentFeeResponse>.Failure("Conflict", "All provided fee types are already assigned to this student.");
                    }

                    // 4. Update Template (FeeStructureDetails) if new types are being introduced to the system
                    foreach (var item in itemsToAssign.Where(x => !existingTemplateTypeIds.Contains(x.feeTypeId)))
                    {
                        entity.FeeStructureDetails.Add(new FeeStructureDetails
                        {
                            Id = Guid.NewGuid().ToString(),
                            FeeTypeId = item.feeTypeId,
                            FeeStructureId = entity.Id,
                            DiscountAmount = item.discountAmount ?? 0,
                            Amount = item.amount,
                            Times = item.times,
                            TotalAmount = item.totalAmount,
                            FeePaidType = item.feePaidType
                        });
                    }

                    // 5. Calculate Financials based ONLY on the items we are adding now
                    decimal currentTotal = itemsToAssign.Sum(x => x.totalAmount);
                    decimal discountAmount = itemsToAssign.Sum(x => x.discountAmount ?? 0);
                    decimal netAmountDue = currentTotal - discountAmount;

                    // 6. Create StudentFee Record
                    var newRecord = new StudentFee(
                        newStudentFeeId,
                        addStudentFeeCommand.studentId,
                        addStudentFeeCommand.feeStructureId,
                        addStudentFeeCommand.classId,
                        discountAmount,
                        addStudentFeeCommand.discountPercentage,
                        currentTotal,
                        netAmountDue,
                        itemsToAssign.Select(item => new StudentFeeDetail(
                            Guid.NewGuid().ToString(),
                            item.feeTypeId,
                            newStudentFeeId,
                            item.discountAmount,
                            item.amount,
                            item.times,
                            item.totalAmount,
                            item.feePaidType,
                            true
                        )).ToList(),
                        true, schoolId, userId, DateTime.UtcNow, "", default
                    );

                    await _unitOfWork.BaseRepository<StudentFee>().AddAsync(newRecord);

                    // 7. Ledger Management
                    var studentData = await _unitOfWork.BaseRepository<StudentData>().GetByGuIdAsync(addStudentFeeCommand.studentId);
                    if (string.IsNullOrEmpty(studentData.LedgerId))
                    {
                        var newLedgerId = Guid.NewGuid().ToString();
                        var ledger = new Ledger(
                            newLedgerId,
                            $"{studentData.FirstName} {studentData.MiddleName} {studentData.LastName}".Replace("  ", " "),
                            DateTime.UtcNow, false, studentData.Address, "", studentData.PhoneNumber, "", "",
                            SubLedgerGroupConstants.SundryDebtors, schoolId, FyId, 0, false, true
                        );
                        await _unitOfWork.BaseRepository<Ledger>().AddAsync(ledger);
                        studentData.LedgerId = newLedgerId;
                    }

                    // 8. Journal Entry
                    var newJournalId = Guid.NewGuid().ToString();
                    var journalDetails = new List<JournalEntryDetails>
                        {
                            // Debit Student Ledger (Receivable)
                            new JournalEntryDetails(Guid.NewGuid().ToString(), newJournalId, studentData.LedgerId, netAmountDue, 0, DateTime.UtcNow, schoolId, FyId, true),
                            // Credit Income Ledger (Revenue)
                            new JournalEntryDetails(Guid.NewGuid().ToString(), newJournalId, entity.LedgerId, 0, currentTotal, DateTime.UtcNow, schoolId, FyId, true)
                        };

                    if (discountAmount > 0)
                    {
                        // Debit Discount (Expense/Contra-Revenue)
                        journalDetails.Add(new JournalEntryDetails(Guid.NewGuid().ToString(), newJournalId, LedgerConstants.DiscountLedgerId, discountAmount, 0, DateTime.UtcNow, schoolId, FyId, true));
                    }

                    var journalData = new JournalEntry(newJournalId, "Student Fee Assigned", DateTime.UtcNow, $"Fees assigned for {studentData.FirstName}", userId, schoolId, DateTime.UtcNow, "", default, "", FyId, true, journalDetails);
                    await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);

                    // 9. Commit
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    return Result<AddStudentFeeResponse>.Success(_mapper.Map<AddStudentFeeResponse>(newRecord));
                }
                catch (Exception ex)
                {
                    // Log properly here
                    throw;
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
                        x.FeeStructureDetails.FirstOrDefault().FeeTypeId == assignMonthlyFeeCommand.feeTypeId &&
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
                                    feeStructure.FeeStructureDetails.Sum(x=>x.TotalAmount),
                                    0
                                ),
                                new AddJournalEntryDetailsRequest(
                                    feeStructure.LedgerId,
                                    0,
                                     feeStructure.FeeStructureDetails.Sum(x=>x.TotalAmount)
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


                IQueryable<StudentFee> query = filter
                    .Include(x=>x.FeeStructure)
                    .ThenInclude(x=>x.FeeStructureDetails).AsQueryable();

                // Student filter
                if (!string.IsNullOrEmpty(filterStudentFeeDTOs.studentId))
                {
                    query = query.Where(x => x.StudentId == filterStudentFeeDTOs.studentId);
                }

                // Date filter
                if (filterStudentFeeDTOs.startDate != null && filterStudentFeeDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterStudentFeeDTOs.startDate,
                        filterStudentFeeDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                // Active filter
                query = query.Where(x => x.IsActive);

                // Grouping + projection
                var filteredResult = query
                    .GroupBy(x => new { x.StudentId, x.ClassId })
                    .Select(g => new
                    {
                        Id = g.Select(x => x.Id).FirstOrDefault(),
                        StudentId = g.Key.StudentId,
                        ClassId = g.Key.ClassId,
                        SchoolId = g.Select(x=>x.SchoolId).FirstOrDefault(),

                        FeeStructureIds = g
                            .Select(x => x.FeeStructure.FeeStructureDetails.FirstOrDefault().FeeType.Name + " -> " + x.DiscountPercentage + "%")
                            .ToList(),

                        TotalAmount = g.Sum(x => x.TotalAmount),

                        TotalPaid = g.Sum(x => x.PaidAmount),

                        DueAmount = g.Sum(x => x.TotalAmount) - g.Sum(x => x.PaidAmount),

                        LatestDate = g.Max(x => x.CreatedAt),
                        PaymentReceipts = g
                        .SelectMany(x => x.Payments)   // flatten all payments
                        .Where(p => p.ReceiptNumber != null) // optional safety
                        .Select(p => p.ReceiptNumber)
                        .Distinct()
                        .ToList(),
                    })
                    .OrderByDescending(x => x.LatestDate)
                    .ToList();

                var responseList = filteredResult
                       .Select(i => new FilterStudentFeeResponse(
                           i.Id.ToString(),
                           i.StudentId,
                           i.FeeStructureIds,
                           i.TotalAmount,
                           i.TotalPaid,
                           i.DueAmount,
                           i.ClassId,
                           i.SchoolId,
                           i.PaymentReceipts.FirstOrDefault()
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
                var studentFeeDetails = await _unitOfWork.BaseRepository<StudentFee>().GetConditionalAsync(
                    x => x.Id == id,
                    query => query.Include(x => x.StudentFeeDetails)
                    );

                var studentFee = studentFeeDetails.FirstOrDefault();

                var studentFeeResponse = new StudentFeeByIdResponse
                        (
                            studentFee.Id,
                            studentFee.StudentId,
                            studentFee.FeeStructureId,
                            studentFee.ClassId,
                            studentFee.DiscountPercentage,
                            studentFee.StudentFeeDetails != null
                                ? studentFee.StudentFeeDetails?
                                .Where(x => x.IsActive == true)
                                .Select(x =>
                                    new UpdateStudentFeeDetailsDTOs
                                    (
                                        x.Id,
                                        x.FeeTypeId,
                                        x.DiscountAmount,
                                        x.Amount,
                                        x.Times,
                                        x.TotalAmount,
                                        x.FeePaidType
                                    )
                                ).ToList()
                                : new List<UpdateStudentFeeDetailsDTOs>()
                        );

                return Result<StudentFeeByIdResponse>.Success(studentFeeResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Class by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<StudentFeeSummaryResponse>>> GetStudentFeeSummary(PaginationRequest paginationRequest, StudentFeeSummaryDTOs studentFeeSummaryDTOs)
        {
            try
            {
                string schoolId = _tokenService.SchoolId().FirstOrDefault();

                var netTotal = await _unitOfWork.BaseRepository<StudentFee>()
                    .GetAsQueryable()
                    .Where(x => x.StudentId == studentFeeSummaryDTOs.studentId &&
                                x.ClassId == studentFeeSummaryDTOs.classId &&
                                x.IsActive && x.SchoolId == schoolId)
                    .SumAsync(x => x.TotalAmount - x.DiscountAmount);

                var payments = await _unitOfWork.BaseRepository<PaymentsRecords>()
                    .GetAsQueryable()
                    .Where(x => x.StudentId == studentFeeSummaryDTOs.studentId &&
                                x.StudentFee.ClassId == studentFeeSummaryDTOs.classId &&
                                x.IsActive && x.Schoolid == schoolId)
                    .OrderBy(x => x.PaymentDate)
                    .ToListAsync();

                var fullList = new List<StudentFeeSummaryResponse>();
                decimal currentBalance = netTotal;

                foreach (var p in payments)
                {
                    decimal amountBeforePayment = currentBalance;

                    currentBalance -= p.AmountPaid;

                    fullList.Add(new StudentFeeSummaryResponse(
                        studentFeeSummaryDTOs.classId,
                        p.AmountPaid,
                        p.PaymentMethod,
                        amountBeforePayment, 
                        currentBalance  ,
                        p.Schoolid,
                        p.ReceiptNumber
                    ));
                }

                fullList.Reverse();

                int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                var pagedItems = fullList
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Result<PagedResult<StudentFeeSummaryResponse>>.Success(new PagedResult<StudentFeeSummaryResponse>
                {
                    Items = pagedItems,
                    TotalItems = fullList.Count,
                    PageIndex = pageIndex,
                    pageSize = pagedItems.Count
                });
            }
            catch (Exception ex)
            {
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

                    var studentFeeDetails = await _unitOfWork.BaseRepository<StudentFee>().
                                GetConditionalAsync(x => x.Id == studentFeeId,
                                query => query.Include(rm => rm.StudentFeeDetails)
                                );
                    var studentFeeToBeUpdated = studentFeeDetails.FirstOrDefault();
                    if (studentFeeToBeUpdated is null)
                    {
                        return Result<UpdateStudentFeeResponse>.Failure("NotFound", "StudentFee not Found");
                    }
                    studentFeeToBeUpdated.ModifiedAt = DateTime.UtcNow;
                    studentFeeToBeUpdated.StudentId = updateStudentFeeCommand.studentId;
                    studentFeeToBeUpdated.FeeStructureId = updateStudentFeeCommand.feeStructureId;
                    studentFeeToBeUpdated.ClassId = updateStudentFeeCommand.classId;
                    studentFeeToBeUpdated.DiscountPercentage = updateStudentFeeCommand.discountPercentage;

                    if (updateStudentFeeCommand.StudentFeeDetailsDTOs != null && updateStudentFeeCommand.StudentFeeDetailsDTOs.Any())
                    {
                        foreach (var detail in updateStudentFeeCommand.StudentFeeDetailsDTOs)
                        {
                            //var existingExamResult = await _unitOfWork.BaseRepository<MarksObtained>().GetByGuIdAsync(detail.Id);

                            var existingStudentFeeDetails = studentFeeToBeUpdated.StudentFeeDetails
                             .FirstOrDefault(x => x.Id == detail.id);

                            var incomingIds = updateStudentFeeCommand.StudentFeeDetailsDTOs
                                .Where(x => !string.IsNullOrEmpty(x.id))
                                .Select(x => x.id)
                                .ToList();

                            var existingDetails = studentFeeToBeUpdated.StudentFeeDetails.ToList();

                            var toBeRemoved = existingDetails
                                .Where(x => !incomingIds.Contains(x.Id))
                                .ToList();

                            foreach (var item in toBeRemoved)
                            {
                                item.IsActive = false;
                                _unitOfWork.BaseRepository<StudentFeeDetail>().Update(item);
                            }

                            if (existingStudentFeeDetails != null)
                            {
                                // update existing
                                existingStudentFeeDetails.FeeTypeId = detail.feeTypeId;
                                existingStudentFeeDetails.DiscountAmount = detail.discountAmount;
                                existingStudentFeeDetails.Amount = detail.amount;
                                existingStudentFeeDetails.Times = detail.times;
                                existingStudentFeeDetails.TotalAmount = detail.totalAmount;
                                existingStudentFeeDetails.FeePaidType = detail.feePaidType;
                                existingStudentFeeDetails.IsActive = true;
                                _unitOfWork.BaseRepository<StudentFeeDetail>().Update(existingStudentFeeDetails);

                            }
                            else
                            {
                                // add new
                                var newStudentFeeDetail = new StudentFeeDetail
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    FeeTypeId = detail.feeTypeId,
                                    DiscountAmount = detail.discountAmount,
                                    Amount = detail.amount,
                                    TotalAmount = detail.totalAmount,
                                    FeePaidType = detail.feePaidType,
                                    StudentFeeId = studentFeeId,
                                    IsActive = true
                                };
                                await _unitOfWork.BaseRepository<StudentFeeDetail>().AddAsync(newStudentFeeDetail);
                            }
                        }

                        await _unitOfWork.SaveChangesAsync();
                    }

                    _mapper.Map(updateStudentFeeCommand, studentFeeToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateStudentFeeResponse
                        (
                            studentFeeId,
                            studentFeeToBeUpdated.StudentId,
                            studentFeeToBeUpdated.FeeStructureId,
                            studentFeeToBeUpdated.ClassId,
                            studentFeeToBeUpdated.DiscountPercentage
                            
                            
                          
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
