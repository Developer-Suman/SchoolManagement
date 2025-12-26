using AutoMapper;
using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure;
using ES.Finances.Application.Finance.Command.Fee.AddStudentFee;
using ES.Finances.Application.Finance.Command.Fee.AssignMonthlyFee;
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

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var add = new StudentFee(
                            newId,
                        addStudentFeeCommand.studentId,
                        addStudentFeeCommand.feeStructureId,
                        addStudentFeeCommand.discount,
                        addStudentFeeCommand.totalAmount,
                        addStudentFeeCommand.paidAmount,
                       DateTime.UtcNow,
                        true,
                        schoolId ?? "",
                        PaidStatus.Pending,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default
                    );

                    await _unitOfWork.BaseRepository<StudentFee>().AddAsync(add);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddStudentFeeResponse>(add);
                    return Result<AddStudentFeeResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding", ex);

                }
            }
        }

        public async Task<Result<AssignMonthlyFeeResponse>> AssignMonthlyFee(AssignMonthlyFeeCommand assignMonthlyFeeCommand)
        {
            try
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



                    var studentIds = students.Select(s => s.Id).ToList();

                    var feeTypeLedger = await _unitOfWork.BaseRepository<Ledger>()
                       .FirstOrDefaultAsync(l =>
                           l.FeeTypeid == assignMonthlyFeeCommand.feeTypeId);

                    var existingStudentLedgers = await _unitOfWork.BaseRepository<Ledger>()
                        .FindBy(l => studentIds.Contains(l.StudentId));

                    if (existingStudentLedgers.Any())
                    {
                        return Result<AssignMonthlyFeeResponse>.Failure(
                            "Monthly fee has already been assigned for this class.");
                    }


                    var ledgerLookup = existingStudentLedgers
                        .ToDictionary(l => l.StudentId);

                    foreach (var student in students)
                    {
                        if (ledgerLookup.ContainsKey(student.Id))
                            continue;

                        var ledger = new Ledger
                            (
                            Guid.NewGuid().ToString(),
                            $"{student.FirstName} {student.LastName}"+ "A/C",
                            DateTime.UtcNow,
                            false,
                            student.Address,
                            "",
                            student.PhoneNumber,
                            "",
                            "",
                            LedgerConstants.AccountsReceivable,
                            schoolId,
                            FyId,
                            0,
                            false,
                            true,
                            student.Id,
                            null

                            );

                        await _unitOfWork.BaseRepository<Ledger>().AddAsync(ledger);
        

                        ledgerLookup.Add(student.Id, ledger);

                        var addJournal = new AddJournalEntryCommand(
                            $"Opening balance for {student.FirstName} {student.LastName}",
                            DateTime.UtcNow.ToString(),
                            "Being opening balance entry",
                            new List<AddJournalEntryDetailsRequest>
                            {
                                new AddJournalEntryDetailsRequest(
                                    ledger.Id,
                                    feeStructure.Amount,
                                    0
                                ),
                                new AddJournalEntryDetailsRequest(
                                    feeTypeLedger.Id,
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
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while assigning monthly fee.", ex);
                }

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
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterStudentFeeResponse(

                    i.Id,
                    i.StudentId,
                    i.FeeStructureId,
                    i.Discount,
                    i.TotalAmount,
                    i.PaidAmount,
                    i.DueDate,
                    i.DueAmount,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


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
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();
                string institutionId = _tokenService.InstitutionId();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var responseList = await _unitOfWork.BaseRepository<StudentFee>()
                    .GetAllWithIncludeQueryable(
                        x => schoolIds.Contains(x.SchoolId) && x.StudentId == studentFeeSummaryDTOs.studentId,
                        x => x.FeeStructure.FeeType
                    )
                    .Select(x => new StudentFeeSummaryResponse
                    (
                        x.FeeStructure.FeeType.Name,
                         x.TotalAmount,
                         x.PaidAmount,
                        x.DueAmount,
                        x.IsPaidStatus
                    ))
                    .AsNoTracking()
                    .ToListAsync();



                PagedResult<StudentFeeSummaryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<StudentFeeSummaryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<StudentFeeSummaryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<StudentFeeSummaryResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching: {ex.Message}", ex);
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
    }
}
