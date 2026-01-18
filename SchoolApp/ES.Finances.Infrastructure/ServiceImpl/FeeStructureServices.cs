using AutoMapper;
using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure;
using ES.Finances.Application.Finance.Command.Fee.AddFeeType;
using ES.Finances.Application.Finance.Command.Fee.UpdateFeeStructure;
using ES.Finances.Application.Finance.Queries.Fee.FeeStructure;
using ES.Finances.Application.Finance.Queries.Fee.FeeStructureByClass;
using ES.Finances.Application.Finance.Queries.Fee.FeeStructureById;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeeStructure;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeetype;
using ES.Finances.Application.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Communication;
using TN.Shared.Domain.Entities.Finance;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;

namespace ES.Finances.Infrastructure.ServiceImpl
{
    public class FeeStructureServices : IFeeStructureServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;

        public FeeStructureServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddFeeStructureResponse>> Add(AddFeeStructureCommand addFeeStructureCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var feeType = await _unitOfWork.BaseRepository<FeeType>()
                        .GetByGuIdAsync(
                            addFeeStructureCommand.feeTypeId
                        );

                    var feeStructure = await _unitOfWork.BaseRepository<FeeStructure>()
                       .FirstOrDefaultAsync(
                           x=>x.ClassId == addFeeStructureCommand.classId &&
                           x.FeeTypeId == addFeeStructureCommand.feeTypeId &&
                           x.NameOfMonths == addFeeStructureCommand.nameOfMonths &&
                           x.FyId == FyId
                       );

                    if (feeStructure is not null)
                    {
                        return Result<AddFeeStructureResponse>.Failure("Conflict", $"Already assigned in the month of {addFeeStructureCommand.nameOfMonths}");
                    }
                    #region AddLedger

                    string ledgerId;

                    if (feeStructure != null &&
                        feeStructure.FeeTypeId == addFeeStructureCommand.feeTypeId)
                    {
                        ledgerId = feeStructure.LedgerId;
                    }
                    else
                    {
                        ledgerId = Guid.NewGuid().ToString();

                        var ledger = new Ledger(
                            ledgerId,
                            feeType.Name + " A/C",
                            DateTime.UtcNow,
                            false,
                            "",
                            "",
                            "",
                            "",
                            "",
                            SubLedgerGroupConstants.IndirectIncomeRevenue,
                            schoolId,
                            FyId,
                            0,
                            false,
                            true
                        );

                        await _unitOfWork.BaseRepository<Ledger>().AddAsync(ledger);
                        await _unitOfWork.SaveChangesAsync();
                    }


                    #endregion


                    var add = new FeeStructure(
                            newId,
                        addFeeStructureCommand.amount,
                        addFeeStructureCommand.classId,
                        FyId,
                        ledgerId,
                        addFeeStructureCommand.feeTypeId,
                        addFeeStructureCommand.nameOfMonths,
                        true,
                        schoolId ?? "",
                        userId,
                        DateTime.UtcNow,
                        "",
                        default
                    );

                    await _unitOfWork.BaseRepository<FeeStructure>().AddAsync(add);


       


                    #region Journal Entries
                    var newJournalId = Guid.NewGuid().ToString();


                    var journalDetails = new List<JournalEntryDetails>();
                    journalDetails.Add(new JournalEntryDetails(
                                        Guid.NewGuid().ToString(),
                                        newJournalId,
                                        LedgerConstants.FeeReceivable,
                                        addFeeStructureCommand.amount,
                                        0,
                                        DateTime.UtcNow,
                                        schoolId,
                                        FyId,
                                        true
                                    ));

                    journalDetails.Add(new JournalEntryDetails(
                                        Guid.NewGuid().ToString(),
                                        newJournalId,
                                        ledgerId,
                                        0,
                                        addFeeStructureCommand.amount,
                                        DateTime.UtcNow,
                                        schoolId,
                                        FyId,
                                        true
                                    ));




                    var journalData = new JournalEntry(
                           newJournalId,
                           "Add Feetypes Voucher",
                           DateTime.UtcNow,
                           "Being FeeTypes are added",
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
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddFeeStructureResponse>(add);
                    return Result<AddFeeStructureResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding", ex);

                }
            }
        }

        public async Task<Result<PagedResult<FeeStructureResponse>>> FeeStructure(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (feeStructure, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<FeeStructure>();

                var finalQuery = feeStructure.Where(x => x.IsActive == true && x.SchoolId == currentSchoolId).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<FeeStructureResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<FeeStructureResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<FeeStructureResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
        }

        public async Task<Result<PagedResult<FilterFeeStructureResponse>>> Filter(PaginationRequest paginationRequest, FilterFeeStructureDTOs filterFeeStructureDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (feeStructure, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<FeeStructure>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
                    ? feeStructure
                    : feeStructure.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterFeeStructureDTOs.startDate, filterFeeStructureDTOs.endDate);

                var filteredResult = filter
                 .Where(x =>
                       (string.IsNullOrEmpty(filterFeeStructureDTOs.classId) || x.ClassId == filterFeeStructureDTOs.classId) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterFeeStructureResponse(

                    i.Id,
                    i.Amount,
                    i.ClassId,
                    i.FyId,
                    i.FeeTypeId,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


                ))
                .ToList();

                PagedResult<FilterFeeStructureResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterFeeStructureResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterFeeStructureResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterFeeStructureResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching result: {ex.Message}", ex);
            }
        }

        public async Task<Result<FeeStructureByIdResponse>> GetFeeStructure(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var feeStructure = await _unitOfWork.BaseRepository<FeeStructure>().GetByGuIdAsync(id);

                var feeStructureResponse = _mapper.Map<FeeStructureByIdResponse>(feeStructure);

                return Result<FeeStructureByIdResponse>.Success(feeStructureResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Notice by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<FeeStructureByClassResponse>>> getFeeStructureBy(PaginationRequest paginationRequest, FeeStructureByClassDTOs feeStructureByClassDTOs)
        {
            try
            {

                var (feeStructure, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<FeeStructure>();

                var finalQuery = feeStructure.Where(
                    x => x.IsActive == true 
                    && x.SchoolId == currentSchoolId 
                    && x.ClassId == feeStructureByClassDTOs.classId).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<FeeStructureByClassResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<FeeStructureByClassResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<FeeStructureByClassResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
        }

        public async Task<Result<UpdateFeeStructureResponse>> Update(string feeStructureId, UpdateFeeStructureCommand updateFeeStructureCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (feeStructureId == null)
                    {
                        return Result<UpdateFeeStructureResponse>.Failure("NotFound", "Please provide valid feeStrcturId");
                    }

                    var feeStrctureToBeUpdated = await _unitOfWork.BaseRepository<FeeStructure>().GetByGuIdAsync(feeStructureId);
                    if (feeStrctureToBeUpdated is null)
                    {
                        return Result<UpdateFeeStructureResponse>.Failure("NotFound", "FeeStructure are not Found");
                    }
                    feeStrctureToBeUpdated.ModifiedAt = DateTime.UtcNow;
                    _mapper.Map(updateFeeStructureCommand, feeStrctureToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateFeeStructureResponse
                        (
                        feeStructureId,
                        feeStrctureToBeUpdated.Amount,
                        feeStrctureToBeUpdated.ClassId,
                        feeStrctureToBeUpdated.FyId,
                        feeStrctureToBeUpdated.FeeTypeId,
                        feeStrctureToBeUpdated.IsActive,
                        feeStrctureToBeUpdated.SchoolId,
                        feeStrctureToBeUpdated.CreatedBy,
                        feeStrctureToBeUpdated.CreatedAt,
                        feeStrctureToBeUpdated.ModifiedBy,
                        feeStrctureToBeUpdated.ModifiedAt

                        );

                    return Result<UpdateFeeStructureResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating", ex);
                }
            }
        }
    }
}
