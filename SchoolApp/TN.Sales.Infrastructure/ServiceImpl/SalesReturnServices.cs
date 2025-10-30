using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NV.Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Authentication.Domain.Static.Roles;
using TN.Inventory.Application.Inventory.Queries.FilterItemsByDate;
using TN.Inventory.Domain.Entities;
using TN.Sales.Application.Sales.Queries.AllSalesDetails;
using TN.Sales.Application.Sales.Queries.FilterSalesQuotationByDate;
using TN.Sales.Application.SalesReturn.Command.AddSalesReturnDetails;
using TN.Sales.Application.SalesReturn.Command.UpdateSalesReturnDetails;
using TN.Sales.Application.SalesReturn.Queries;
using TN.Sales.Application.SalesReturn.Queries.FilterSalesReturnDetailsByDate;
using TN.Sales.Application.SalesReturn.Queries.GetAllSalesReturnItems;
using TN.Sales.Application.SalesReturn.Queries.GetSalesReturnDetailsById;
using TN.Sales.Application.ServiceInterface;
using TN.Sales.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Payments;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using static TN.Authentication.Domain.Entities.SchoolSettings;
using static TN.Sales.Domain.Entities.SalesDetails;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Sales.Infrastructure.ServiceImpl
{
    public class SalesReturnServices : ISalesReturnServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConverterHelper;
        private readonly IBillNumberGenerator _billNumberGenerator;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IMediator _mediator;
        private readonly FiscalContext _fiscalContext;
        private readonly ISettingServices _settingServices;
        public SalesReturnServices(IMediator mediator,ISettingServices settingServices, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, IBillNumberGenerator billNumberGenerator, IDateConvertHelper dateConvertHelper, IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _billNumberGenerator = billNumberGenerator;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _dateConverterHelper = dateConvertHelper;
            _unitOfWork = unitOfWork;
            _fiscalContext = fiscalContext;
            _mapper = mapper;
            _mediator = mediator;
            _settingServices = settingServices;
        }

        public async Task<Result<AddSalesReturnDetailsResponse>> Add(AddSalesReturnDetailsCommand request)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    string newId = Guid.NewGuid().ToString();
                    string userId = _tokenService.GetUserId();
                    string schoolId = _tokenService.SchoolId().FirstOrDefault() ?? string.Empty;

                    var school = await _unitOfWork.BaseRepository<School>().GetByGuIdAsync(schoolId);
                    if (school == null)
                    {
                        return Result<AddSalesReturnDetailsResponse>.Failure("Invalid SchoolId. School does not exist.");
                    }


                    DateTime returnedDate = request.returnDate == default
                  ? DateTime.Today
                  : await _dateConverterHelper.ConvertToEnglish(request.returnDate);


                    var fiscalYear = await _settingServices.GetCurrentFiscalYearBy(schoolId, default);




                    var schoolSettings = await _unitOfWork.BaseRepository<SchoolSettings>().FirstOrDefaultAsync(x => x.SchoolId == schoolId);
                    if (schoolSettings == null)
                    {
                        return Result<AddSalesReturnDetailsResponse>.Failure("Invalid SchoolId. School does not exist.");
                    }
                    string salesReturnNumber = "";
                    if (schoolSettings.SalesReturnNumberType == PurchaseSalesReturnNumberType.Manual)
                    {
                        salesReturnNumber = request.salesReturnNumber!;
                    }
                    else
                    {
                        salesReturnNumber = await _billNumberGenerator.GenerateTransactionNumber(schoolId, "SALESRETURN", fiscalYear.Data.fyName);
                    }



                    bool incomeNumberExists = await _unitOfWork.BaseRepository<SalesReturnDetails>()
                    .AnyAsync(p => p.SalesReturnNumber == salesReturnNumber && p.SchoolId == schoolId);



                    if (incomeNumberExists)
                    {
                        return Result<AddSalesReturnDetailsResponse>.Failure($"SalesReturn number '{salesReturnNumber}' already exists for this company.");
                    }



                    var salesDetails = await _unitOfWork.BaseRepository<SalesDetails>().GetByGuIdAsync(request.salesDetailsId);


                    #region Journal Entries

                    string newJournalId = Guid.NewGuid().ToString();

                    decimal grossReturn = request.SalesReturnItems.Sum(x => x.returnTotalAmount);

                    decimal vatAmount = request.taxAdjustment;
                    decimal discount = request.discount;

                    decimal totalRefundAmount = grossReturn - discount + vatAmount;

                    var journalDetails = new List<JournalEntryDetails>
                        {
                            new JournalEntryDetails(
                                Guid.NewGuid().ToString(),
                                newJournalId,
                                LedgerConstants.StockLedgerId,
                                grossReturn,
                                0,
                                returnedDate,
                                schoolId,
                                _fiscalContext.CurrentFiscalYearId
                            )
                        };

                    if (vatAmount > 0)
                    {
                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            LedgerConstants.VATLedgerId,
                            vatAmount,
                            0,
                            returnedDate,
                            schoolId,
                            _fiscalContext.CurrentFiscalYearId
                        ));
                    }

                    if (discount > 0)
                    {
                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            LedgerConstants.DiscountLedgerId,
                            0,
                            discount,
                            returnedDate,
                            schoolId,
                            _fiscalContext.CurrentFiscalYearId
                        ));
                    }


                    var specialPaymentId = "d1e43e64-9d48-4c85-83f3-3d2d5c9f6c44";
                    var hasPayment = !string.IsNullOrEmpty(request.paymentId);
                    var isSpecial = request.paymentId == specialPaymentId;
                    DateTime entryDates = await _dateConverterHelper.ConvertToEnglish(request.returnDate);
                    if (hasPayment)
                    {
                        var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>()
                            .GetByGuIdAsync(request.paymentId);

                        var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                            .GetByGuIdAsync(paymentMethod.SubLedgerGroupsId)
                            ?? throw new Exception("Ledger group not found.");

                        var ledger = await _unitOfWork.BaseRepository<Ledger>()
                            .FirstOrDefaultAsync(x => x.SubLedgerGroupId == subledgerGroup.Id);

                        switch (subledgerGroup.Id)
                        {
                            case SubLedgerGroupConstants.CashInHands:
                                AddJournal(LedgerConstants.CashLedgerId, 0, totalRefundAmount);
                                break;

                            case SubLedgerGroupConstants.BankAccounts:
                                AddJournal(LedgerConstants.BankLedgerId, 0, totalRefundAmount);
                                break;


                            default:
                                AddJournal(ledger.Id, 0, totalRefundAmount);
                                break;
                        }
                    }



                    // Helper method
                    void AddJournal(string ledgerId, decimal debit, decimal credit)
                    {
                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            ledgerId,
                            debit,
                            credit,
                            entryDates,
                            schoolId,
                            _fiscalContext.CurrentFiscalYearId
                        ));
                    }




                    //Return Cogs
                    journalDetails.Add(new JournalEntryDetails(
                     Guid.NewGuid().ToString(),
                     newJournalId,
                     LedgerConstants.CostOfGoodSoldLedgerId,
                     0,
                     grossReturn,
                     returnedDate,
                     schoolId,
                     _fiscalContext.CurrentFiscalYearId
                 ));

                    journalDetails.Add(new JournalEntryDetails(
                       Guid.NewGuid().ToString(),
                       newJournalId,
                       LedgerConstants.StockLedgerId,
                       grossReturn,
                       0,
                       returnedDate,
                       schoolId,
                       _fiscalContext.CurrentFiscalYearId
                   ));



                    var debit = journalDetails.Sum(x => x.DebitAmount);
                    var credit = journalDetails.Sum(x => x.CreditAmount);

                    if (journalDetails.Sum(x => x.DebitAmount) != journalDetails.Sum(x => x.CreditAmount))
                    {
                        throw new InvalidOperationException("Journal entry is unbalanced.");
                    }


                    var journalData = new JournalEntry
                       (
                           newJournalId,
                           "Purchase Return Vouchers",
                           returnedDate,
                           "Being Item purchase Returned ",
                           userId,
                           schoolId,
                           DateTime.UtcNow,
                           "",
                           default,
                           salesDetails.BillNumber,
                           FyId,
                           journalDetails
                       );

                    await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);
                    #endregion



                    if (salesDetails == null)
                        throw new Exception("SalesDetails not found.");

                    var salesReturnDetailsData = new SalesReturnDetails(
                            newId,
                       request.salesDetailsId,
                       returnedDate,
                       request.totalReturnAmount,
                       request.taxAdjustment,
                       request.discount,
                       request.netReturnAmount,
                       request.reason,
                       schoolId,
                       salesDetails.StockCenterId,
                       userId,
                       DateTime.UtcNow,
                       "",
                       default,
                       salesDetails.LedgerId,
                       newJournalId,
                       salesReturnNumber,
                      request.SalesReturnItems?
                        .Where(d => !string.IsNullOrWhiteSpace(d.salesItemsId))
                        .Select(d => new SalesReturnItems(
                            Guid.NewGuid().ToString(),
                            newId,
                            d.salesItemsId,
                            d.returnQuantity,
                            d.returnUnitPrice,
                            d.returnTotalAmount,
                            userId,
                            DateTime.Now.ToString(),
                            "",
                            ""
                        )).ToList() ?? new List<SalesReturnItems>()

                    );
                    await _unitOfWork.BaseRepository<SalesReturnDetails>().AddAsync(salesReturnDetailsData);

                    var quantityReturned = salesReturnDetailsData.SalesReturnItems.Sum(x => x.ReturnQuantity);

                    var quantityToBeReturnedItemList = await _unitOfWork.BaseRepository<SalesDetails>().GetConditionalAsync
                        (x => x.Id == request.salesDetailsId,
                        query => query.Include(x => x.SalesItems));

                    var quantityToBeReturned = quantityToBeReturnedItemList
                        .SelectMany(x => x.SalesItems)
                        .Sum(item => item.Quantity);

                    var salesDetailsToBeUpdated = await _unitOfWork.BaseRepository<SalesDetails>().GetByGuIdAsync(request.salesDetailsId);

                    if (quantityReturned == quantityToBeReturned && quantityReturned > 0)
                    {
                        salesDetailsToBeUpdated.Status = SalesStatus.Returned;
                    }
                    else if (quantityReturned < quantityToBeReturned && quantityReturned > 0)
                    {
                        salesDetailsToBeUpdated.Status = SalesStatus.PartiallyReturned;
                    }
                    _unitOfWork.BaseRepository<SalesDetails>().Update(salesDetailsToBeUpdated);


         


                    salesDetails.GrandTotalAmount += request.netReturnAmount;



                    if (salesDetails.GrandTotalAmount == 0)
                    {
                        _unitOfWork.BaseRepository<SalesDetails>().Delete(salesDetails);
                    }
                    salesDetails.VatAmount =
                    salesDetails.VatAmount + request.taxAdjustment;

                    salesDetails.DiscountAmount =
                    salesDetails.DiscountAmount + request.discount;



                    if (request.SalesReturnItems != null && request.SalesReturnItems.Any())
                    {
                        foreach (var detail in request.SalesReturnItems)
                        {
                            var existingSales = await _unitOfWork.BaseRepository<SalesItems>().GetByGuIdAsync(detail.salesItemsId);


                            if (existingSales != null)
                            {
                                if (detail.returnQuantity <= 0 || detail.returnQuantity > existingSales.Quantity)
                                    throw new InvalidOperationException("Invalid return quantity.");


                                existingSales.Quantity -= detail.returnQuantity;
                                existingSales.Price = detail.returnQuantity > 0
                                ? detail.returnTotalAmount / detail.returnQuantity
                                : 0;
                                existingSales.Amount -= detail.returnTotalAmount;
                                if (existingSales.Quantity == 0)
                                {
                                    existingSales.IsActive = false;
                                    _unitOfWork.BaseRepository<SalesItems>().Update(existingSales);
                                }
                                else
                                    _unitOfWork.BaseRepository<SalesItems>().Update(existingSales);
                            }

                            #region ItemInstances
                            var itemInstance = await _unitOfWork.BaseRepository<ItemInstance>()
                                .FirstOrDefaultAsync(x => x.SalesItemsId == detail.salesItemsId && x.ItemsId == detail.itemsId);

                            if (itemInstance != null)
                            {
                                _unitOfWork.BaseRepository<ItemInstance>()
                                     .Delete(itemInstance);
                            }

                            #endregion

                            #region Inventory

                            var salesItemIds = request.SalesReturnItems
                                .Select(x => x.salesItemsId)
                                .Distinct()
                                .ToList();

                            var ItemIds = request.SalesReturnItems
                                .Select(x => x.itemsId)
                                .Distinct()
                                .ToList();

                            var inventories = await _unitOfWork.BaseRepository<Inventories>()
                                .FirstOrDefaultAsync(x => salesItemIds.Contains(x.SalesItemsId) && ItemIds.Contains(x.ItemId));




                            if (inventories != null)
                            {
                                inventories.QuantityOut += detail.returnQuantity;
                                inventories.AmountOut += detail.returnTotalAmount;

                                if (inventories.QuantityIn == 0)
                                    _unitOfWork.BaseRepository<Inventories>().Delete(inventories);
                                else
                                    _unitOfWork.BaseRepository<Inventories>().Update(inventories);
                            }
                            else
                            {
                                //Incomplete for salesItemsId
                                var inventoryLogs = await _unitOfWork.BaseRepository<InventoriesLogs>()
                                    .FirstOrDefaultAsync(x => salesItemIds.Contains(x.PurchaseItemsId) && ItemIds.Contains(x.ItemId));

                                if (inventoryLogs != null)
                                {
                                    inventoryLogs.QuantityIn += detail.returnQuantity;
                                    inventoryLogs.AmountIn += detail.returnTotalAmount;

                                    if (inventoryLogs.QuantityIn == 0)
                                        _unitOfWork.BaseRepository<InventoriesLogs>().Delete(inventoryLogs);
                                    else
                                        _unitOfWork.BaseRepository<InventoriesLogs>().Update(inventoryLogs);
                                }


                            }

                            #endregion


                        }
                    }
               
                  


                    #region Payment Added
                    PaymentsDetails payment;

                    var paymentMethods = await _unitOfWork.BaseRepository<PaymentMethod>()
                       .GetByGuIdAsync(request.paymentId);

                    if (paymentMethods.SubLedgerGroupsId == SubLedgerGroupConstants.BankAccounts)
                    {
                        payment = new ChequePayment(
                            id: Guid.NewGuid().ToString(),
                            transactionType: TransactionType.SalesReturn,
                            transactionDate: DateTime.Now,
                            totalAmount: totalRefundAmount,
                            transactionDetailsId: newId,
                            paymentMethodId: request.paymentId,
                            chequeNumber: request.chequeNumber,
                            bankName: request.bankName,
                            accountName: request.accountName,
                            schoolId: schoolId,
                            chequeDate: DateTime.Now
                        );
                    }
                    else
                    {
                        payment = new PaymentsDetails(
                            id: Guid.NewGuid().ToString(),
                            transactionType: TransactionType.SalesReturn,
                            transactionDate: DateTime.Now,
                            totalAmount: totalRefundAmount,
                            transactionDetailsId: newId,
                            paymentMethodId: request.paymentId,
                            schoolId: schoolId
                        );
                    }

                    await _unitOfWork.BaseRepository<PaymentsDetails>().AddAsync(payment);

                    #endregion



                    await _unitOfWork.SaveChangesAsync();




                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddSalesReturnDetailsResponse>(salesReturnDetailsData);
                    return Result<AddSalesReturnDetailsResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Sales Return Details ", ex);

                }
            }

        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {

                var salesReturnDetails = await _unitOfWork.BaseRepository<SalesReturnDetails>().GetByGuIdAsync(id);
                if (salesReturnDetails is null)
                {
                    return Result<bool>.Failure("NotFound", "sales ReturnDetails  Cannot be Found");
                }


                var originalJournals = await _unitOfWork.BaseRepository<JournalEntry>()
                    .GetConditionalAsync(x => x.Id == salesReturnDetails.JournalEntriesId,
                    query => query.Include(j => j.JournalEntryDetails)
                    );
                var originalJournal = originalJournals.FirstOrDefault();
                _unitOfWork.BaseRepository<JournalEntry>().Delete(originalJournal);
                _unitOfWork.BaseRepository<SalesReturnDetails>().Delete(salesReturnDetails);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting sales Return Details having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllSalesReturnDetailsByQueryResponse>>> GetAllSalesReturnDetails(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var salesReturnDetails = await _unitOfWork.BaseRepository<SalesReturnDetails>()
              .GetConditionalAsync(
                null,
              query => query.Include(rm => rm.SalesReturnItems)
              .ThenInclude(i => i.SalesItems)
              ) ?? new List<SalesReturnDetails>();



                var salesReturnDetailsResponses = salesReturnDetails.Select(salesReturn =>
                {
                    decimal subTotalAmount = salesReturn.SalesReturnItems?.Sum(i => i.ReturnTotalPrice) ?? 0m;

                    decimal? taxableAmount = subTotalAmount - salesReturn.Discount;

                    return new GetAllSalesReturnDetailsByQueryResponse(
                        salesReturn.Id,
                        salesReturn.SalesDetailsId,
                        salesReturn.ReturnDate,
                        salesReturn.TotalReturnAmount,
                        salesReturn.TaxAdjustment,
                        salesReturn.Discount,
                        salesReturn.NetReturnAmount,
                        salesReturn.Reason,
                        salesReturn.CreatedBy,
                        salesReturn.CreatedAt,
                        salesReturn.ModifiedBy,
                        salesReturn.ModifiedAt,
                        salesReturn.SalesReturnItems?.Count() ?? 0,
                        salesReturn.LedgerId,
                        salesReturn.SchoolId,
                        salesReturn.StockCenterId,
                          taxableAmount,
      subTotalAmount,
                        salesReturn.SalesReturnItems?.Select(items => new SalesReturnItemsDto(
                            items.Id,
                            items.SalesReturnDetailsId,
                            items.SalesItemsId,
                            items.SalesItems.ItemId,
                            items.ReturnQuantity,
                            items.ReturnUnitPrice,
                            items.ReturnTotalPrice
                        )).ToList() ?? new List<SalesReturnItemsDto>()
                    );
                }).ToList();



                var institutionId = _tokenService.InstitutionId() ?? string.Empty;

                var isSuperAdmin = _tokenService.GetRole() == Role.SuperAdmin;


                var filterSalesReturnDetails = isSuperAdmin ? salesReturnDetailsResponses : salesReturnDetailsResponses.Where(x => x.schoolId == _tokenService.SchoolId().FirstOrDefault());

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                   .GetConditionalFilterType(
                       x => x.InstitutionId == institutionId,
                       query => query.Select(c => c.Id)
                   );


                if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(_tokenService.SchoolId().FirstOrDefault()))
                {
                    var filteredSalesReturnEntries = await _unitOfWork.BaseRepository<SalesReturnDetails>()
                        .GetConditionalAsync(x => schoolIds.Contains(x.SchoolId),
                        query => query.Include(j => j.SalesReturnItems)
                        );

                    filterSalesReturnDetails = filteredSalesReturnEntries
                        .Select(salesReturn =>
                        {
                            decimal subTotalAmount = salesReturn.SalesReturnItems?.Sum(i => i.ReturnTotalPrice) ?? 0m;

                            decimal? taxableAmount = subTotalAmount - salesReturn.Discount;
                            return new GetAllSalesReturnDetailsByQueryResponse(
                                     salesReturn.Id,
                             salesReturn.SalesDetailsId,
                             salesReturn.ReturnDate,
                             salesReturn.TotalReturnAmount,
                             salesReturn.TaxAdjustment,
                             salesReturn.Discount,
                             salesReturn.NetReturnAmount,
                             salesReturn.Reason,
                             salesReturn.CreatedBy,
                             salesReturn.CreatedAt,
                             salesReturn.ModifiedBy,
                             salesReturn.ModifiedAt,
                             salesReturn.SalesReturnItems?.Count() ?? 0,
                             salesReturn.LedgerId,
                             salesReturn.SchoolId,
                             salesReturn.StockCenterId,
                                   taxableAmount,
      subTotalAmount,
                             salesReturn.SalesReturnItems?.Select(items => new SalesReturnItemsDto(
                                 items.Id,
                                 items.SalesReturnDetailsId,
                                 items.SalesItemsId,
                                 items.SalesItems.Item.Id,
                                 items.ReturnQuantity,
                                 items.ReturnUnitPrice,
                                 items.ReturnTotalPrice
                             )).ToList() ?? new List<SalesReturnItemsDto>()
                         );
                        }
                        ).ToList();


                }

                var totalItems = filterSalesReturnDetails.Count();

                var paginatedSalesReturnDetailsEntries = paginationRequest != null && paginationRequest.IsPagination
                    ? filterSalesReturnDetails
                        .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                        .Take(paginationRequest.pageSize)
                        .ToList()
                    : filterSalesReturnDetails.ToList();

                var pagedResult = new PagedResult<GetAllSalesReturnDetailsByQueryResponse>
                {
                    Items = paginatedSalesReturnDetailsEntries,
                    TotalItems = totalItems,
                    PageIndex = paginationRequest?.pageIndex ?? 1,
                    pageSize = paginationRequest?.pageSize ?? totalItems
                };

                return Result<PagedResult<GetAllSalesReturnDetailsByQueryResponse>>.Success(pagedResult);

            }
            catch (Exception ex)

            {
                throw new Exception("An error occurred while fetching all Sales Return Details", ex);
            }
        }

        public async Task<Result<PagedResult<GetSalesReturnDetailsFilterQueryResponse>>> SalesReturnDetailsFilters(PaginationRequest paginationRequest, FilterSalesReturnDetailsDTOs filterSalesReturnDetailsDTOs)
        {

            try
            {
                // Get scoped user data
                var (ledger, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<SalesReturnDetails>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                // Parse date range using Nepali date conversion
                DateTime? startDateUtc = null;
                DateTime? endDateUtc = null;

                if (!string.IsNullOrWhiteSpace(filterSalesReturnDetailsDTOs.startDate?.Trim()))
                {
                    startDateUtc = await _dateConverterHelper.ConvertToEnglish(filterSalesReturnDetailsDTOs.startDate.Trim());
                    if (DateTime.TryParse(filterSalesReturnDetailsDTOs.startDate, out var tempStart) && tempStart.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = startDateUtc?.AddDays(1);
                    }
                }

                if (!string.IsNullOrWhiteSpace(filterSalesReturnDetailsDTOs.endDate?.Trim()))
                {
                    endDateUtc = await _dateConverterHelper.ConvertToEnglish(filterSalesReturnDetailsDTOs.endDate.Trim());
                    if (DateTime.TryParse(filterSalesReturnDetailsDTOs.endDate, out var tempEnd) && tempEnd.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = endDateUtc?.AddDays(1);
                    }
                }

                // If both dates are invalid, default to today's full day
                bool isStartInvalid = !startDateUtc.HasValue || startDateUtc == DateTime.MinValue;
                bool isEndInvalid = !endDateUtc.HasValue || endDateUtc == DateTime.MinValue;

                if (isStartInvalid && isEndInvalid)
                {
                    var today = DateTime.UtcNow.Date;
                    startDateUtc = today;
                    endDateUtc = today.AddDays(1);
                }
                else if (!isStartInvalid && isEndInvalid)
                {
                    endDateUtc = startDateUtc.Value.AddDays(1);
                }
                else if (isStartInvalid && !isEndInvalid)
                {
                    startDateUtc = endDateUtc.Value.AddDays(-1);
                }

               
                // Filter by user
                var userId = _tokenService.GetUserId();

                var salesReturnResult = await _unitOfWork.BaseRepository<SalesReturnDetails>().GetConditionalAsync(
                    predicate: x =>
                        x.CreatedBy == userId &&
                        (string.IsNullOrEmpty(filterSalesReturnDetailsDTOs.ledgerId) || x.LedgerId.ToLower().Contains(filterSalesReturnDetailsDTOs.ledgerId.ToLower())) &&
                        (startDateUtc == null || x.CreatedAt >= startDateUtc) &&
                        (endDateUtc == null || x.CreatedAt < endDateUtc),
                    queryModifier: q => q.Include(r => r.SalesReturnItems)
                    .ThenInclude(x=>x.SalesItems)
                    .ThenInclude(x=>x.Item)
                );

                // Mapping
                var responseList = salesReturnResult.Select(sr =>
                {
                    decimal subTotalAmount = sr.SalesReturnItems?.Sum(i => i.ReturnTotalPrice) ?? 0m;

                    decimal? taxableAmount = subTotalAmount - sr.Discount;

                    return new GetSalesReturnDetailsFilterQueryResponse(
                        sr.Id,
                        sr.SalesDetailsId,
                        sr.ReturnDate,
                        sr.TotalReturnAmount,
                        sr.TaxAdjustment,
                        sr.Discount,
                        sr.NetReturnAmount,
                        sr.Reason,
                        sr.CreatedBy,
                        sr.CreatedAt,
                        sr.ModifiedBy,
                        sr.ModifiedAt,
                        sr.LedgerId,
                        sr.SchoolId,
                        sr.StockCenterId,
                              taxableAmount,
      subTotalAmount,
                        sr.SalesReturnItems?.Select(items => new SalesReturnItemsDto(
                            items.Id,
                            items.SalesReturnDetailsId,
                            items.SalesItemsId,
                            items.SalesItems?.Item?.Id ?? string.Empty,
                            items.ReturnQuantity,
                            items.ReturnUnitPrice,
                            items.ReturnTotalPrice
                        )).ToList() ?? new List<SalesReturnItemsDto>()
                    );
                }).ToList();

                PagedResult<GetSalesReturnDetailsFilterQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetSalesReturnDetailsFilterQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetSalesReturnDetailsFilterQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }

                return Result<PagedResult<GetSalesReturnDetailsFilterQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching sales return details: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllSalesReturnItemsByQueryResponse>>> GetAllSalesReturnItems(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var salesReturnItems = await _unitOfWork.BaseRepository<SalesReturnItems>().GetAllAsyncWithPagination();
                var salesReturnItemsPagedResult = await salesReturnItems.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allSalesReturnItemsDisplay = _mapper.Map<PagedResult<GetAllSalesReturnItemsByQueryResponse>>(salesReturnItemsPagedResult.Data);

                return Result<PagedResult<GetAllSalesReturnItemsByQueryResponse>>.Success(allSalesReturnItemsDisplay);

            }
            catch (Exception ex)

            {
                throw new Exception("An error occurred while fetching all Sales Return Items", ex);
            }
        }

        public async Task<Result<GetSalesReturnDetailsByIdQueryResponse>> GetSalesReturnDetailsById(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var salesReturnDetails = await _unitOfWork.BaseRepository<SalesReturnDetails>().GetByGuIdAsync(id);

                var salesPayment = await _unitOfWork.BaseRepository<ChequePayment>()
                    .GetConditionalAsync(x => x.TransactionDetailsId == id && x.TransactionType == TransactionType.SalesReturn);

                var salesPaymentDetails = salesPayment.FirstOrDefault();

                if (salesReturnDetails == null)
                {
                    return Result<GetSalesReturnDetailsByIdQueryResponse>.Failure("Sales return details not found.");
                }

                var salesReturnDetailsResponse = _mapper.Map<GetSalesReturnDetailsByIdQueryResponse>(salesReturnDetails);

                var updatedResponse = salesPaymentDetails is not null
                       ? salesReturnDetailsResponse with
                       {
                           chequeNumber = salesPaymentDetails.ChequeNumber,
                           bankName = salesPaymentDetails.BankName,
                           accountName = salesPaymentDetails.AccountName
                       }
                       : salesReturnDetailsResponse;

                return Result<GetSalesReturnDetailsByIdQueryResponse>.Success(updatedResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching SalesReturn Details by using Id", ex);
            }
        }

        public async Task<Result<UpdateSalesReturnDetailsResponse>> Update(UpdateSalesReturnDetailsCommand request, string id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        return Result<UpdateSalesReturnDetailsResponse>.Failure("NotFound", "Please provide valid sales return Details id");
                    }

                    var salesReturnDetailsTobeUpdated = await _unitOfWork.BaseRepository<SalesReturnDetails>().GetByGuIdAsync(id);
                    if (salesReturnDetailsTobeUpdated is null)
                    {
                        return Result<UpdateSalesReturnDetailsResponse>.Failure("NotFound", "SalesReturn details  are not Found");
                    }

                    _mapper.Map(request, salesReturnDetailsTobeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateSalesReturnDetailsResponse
                        (
                            id,
                            salesReturnDetailsTobeUpdated.SalesDetailsId,
                            salesReturnDetailsTobeUpdated.ReturnDate,
                            salesReturnDetailsTobeUpdated.TotalReturnAmount,
                            salesReturnDetailsTobeUpdated.TaxAdjustment,
                            salesReturnDetailsTobeUpdated.NetReturnAmount,
                            salesReturnDetailsTobeUpdated.Reason,
                            salesReturnDetailsTobeUpdated.CreatedBy,
                            salesReturnDetailsTobeUpdated.CreatedAt,
                            salesReturnDetailsTobeUpdated.ModifiedBy,
                            salesReturnDetailsTobeUpdated.ModifiedAt

                        );

                    return Result<UpdateSalesReturnDetailsResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("an error occurred while updating sales return Details ");
                }
            }
        }
    }
}
