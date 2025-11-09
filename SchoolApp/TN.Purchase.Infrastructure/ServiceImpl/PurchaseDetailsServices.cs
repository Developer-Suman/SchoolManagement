
using Autofac.Core;
using AutoMapper;
using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NV.Payment.Domain.Entities;
using System.Linq;
using System.Net;
using System.Transactions;
using TN.Account.Application.ServiceInterface;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Authentication.Domain.Static.Roles;
using TN.Inventory.Application.Inventory.Queries.FilterItemsByDate;
using TN.Inventory.Domain.Entities;
using TN.Purchase.Application.Purchase.Command.AddPurchaseDetails;
using TN.Purchase.Application.Purchase.Command.AddPurchaseItems;
using TN.Purchase.Application.Purchase.Command.QuotationToPurchase;
using TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationByCompany;
using TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationBySchool;
using TN.Purchase.Application.Purchase.Command.UpdatePurchaseDetails;
using TN.Purchase.Application.Purchase.Events.StockUpdated;
using TN.Purchase.Application.Purchase.Queries.BillNumberGenerationBySchool;
using TN.Purchase.Application.Purchase.Queries.FilterPurchaseDetailsByDate;
using TN.Purchase.Application.Purchase.Queries.FilterPurchaseQuotationByDate;
using TN.Purchase.Application.Purchase.Queries.GetPurchaseDetailsByRefNo;
using TN.Purchase.Application.Purchase.Queries.GetPurchaseQuotationById;
using TN.Purchase.Application.Purchase.Queries.Purchase;
using TN.Purchase.Application.Purchase.Queries.PurchaseDetailsById;
using TN.Purchase.Application.PurchaseReturn.Command.AddPurchaseReturnDetails;
using TN.Purchase.Application.PurchaseReturn.Command.UpdatePurchaseReturnDetails;
using TN.Purchase.Application.PurchaseReturn.Queries.AllPurchaseReturnDetails;
using TN.Purchase.Application.PurchaseReturn.Queries.FilterPurchaseReturnDetailsByDate;
using TN.Purchase.Application.PurchaseReturn.Queries.PurchaseReturnDetailsById;
using TN.Purchase.Application.ServiceInterface;
using TN.Purchase.Domain.Entities;
using TN.Sales.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Command.CalculationBillSundry;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Payments;
using TN.Shared.Domain.Entities.Purchase;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using TN.Shared.Infrastructure.Repository;
using static TN.Authentication.Domain.Entities.School;
using static TN.Authentication.Domain.Entities.SchoolSettings;
using static TN.Inventory.Domain.Entities.Inventories;
using static TN.Purchase.Domain.Entities.PurchaseDetails;
using static TN.Shared.Domain.Entities.Purchase.PurchaseQuotationDetails;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;


namespace TN.Purchase.Infrastructure.ServiceImpl
{
    public class PurchaseDetailsServices : IPurchaseDetailsServices
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
        private readonly IBillSundryServices _billSundryServices;

        public PurchaseDetailsServices(IBillSundryServices billSundryServices, IMediator mediator,ISettingServices settingServices,FiscalContext fiscalContext,IGetUserScopedData getUserScopedData, IBillNumberGenerator billNumberGenerator, IDateConvertHelper dateConvertHelper, IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _billNumberGenerator = billNumberGenerator;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _dateConverterHelper = dateConvertHelper;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
            _fiscalContext = fiscalContext;
            _settingServices = settingServices;
            _billSundryServices = billSundryServices;
        }
        public async Task<Result<AddPurchaseDetailsResponse>> Add(AddPurchaseDetailsCommand request)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    string newId = Guid.NewGuid().ToString();
                    string userId = _tokenService.GetUserId();
                    string schoolId = _tokenService.SchoolId().FirstOrDefault() ?? string.Empty;

                    var fiscalYear = await _settingServices.GetCurrentFiscalYearBy(schoolId, default);

                    var school = await _unitOfWork.BaseRepository<School>().GetByGuIdAsync(schoolId);
                    if (school == null)
                    {
                        return Result<AddPurchaseDetailsResponse>.Failure("Invalid SchoolId. School does not exist.");
                    }

                    string billNumber = "";
                    if (school.BillNumberGenerationTypeForPurchase == BillNumberGenerationType.Manual)
                    {
                        billNumber = request.billNumber;
                    }
                    else
                    {
                        billNumber = await _billNumberGenerator.GenerateBillNumberAsync(schoolId, "purchase", fiscalYear.Data.fyName);
                    }



                    var schoolSettings = await _unitOfWork.BaseRepository<SchoolSettings>().FirstOrDefaultAsync(x => x.SchoolId == schoolId);
                    if (schoolSettings == null)
                    {
                        return Result<AddPurchaseDetailsResponse>.Failure("Invalid SchoolId. School does not exist.");
                    }
                    string referenceNumber = "";
                    if (schoolSettings.PurchaseReferences == PurchaseReferencesType.Manual)
                    {
                        referenceNumber = request.referenceNumber!;
                    }
                    else
                    {
                        referenceNumber = await _billNumberGenerator.GenerateReferenceNumber(schoolId, ReferenceType.Purchase);
                    }



                    DateTime entryDate = request.date == null
                  ? DateTime.Today
                  : await _dateConverterHelper.ConvertToEnglish(request.date);
                    string newJournalId = "";


                    var resultDTOs = new AddPurchaseDetailsResponse();
                    if (request.isPurchase)
                    {

                        newJournalId = Guid.NewGuid().ToString();

                        decimal subTotal = request.subTotalAmount ?? 0;
                        decimal vatAmount = request.vatAmount ?? 0;
                        decimal discountAmount = request.discountAmount ?? 0;
                        decimal taxableAmount = request.taxableAmount ?? 0;
                        decimal amountAfterVat = request.amountAfterVat ?? 0;

                        var journalDetails = new List<JournalEntryDetails>();
                        var billSundryResponses = new List<CalculationBillResponseDTOs>();

                        decimal billSundryDebitTotal = 0;
                        decimal billSundryCreditTotal = 0;

                        // ------------------- BILL SUNDRY -------------------
                        if (request.BillSundryIds != null && request.BillSundryIds.Any())
                        {
                            foreach (var bsId in request.BillSundryIds.Where(x => !string.IsNullOrEmpty(x.billSundryIds)))
                            {
                                var calcDto = new CalculationBIllDTOs(bsId.billSundryIds, subTotal, taxableAmount, amountAfterVat);

                                var bsConfig = await _unitOfWork.BaseRepository<BillSundry>().GetByGuIdAsync(bsId.billSundryIds);
                                bsConfig.DefaultValue = bsId.rate ?? 0;
                                _unitOfWork.BaseRepository<BillSundry>().Update(bsConfig);



                                var calcResult = await _billSundryServices.CalculateBillSundry(calcDto);
                                if (!calcResult.IsSuccess)
                                    return Result<AddPurchaseDetailsResponse>.Failure("Calculation Error");

                                var bsResponse = calcResult.Data;
                                billSundryResponses.Add(bsResponse);

                     

                                if (bsConfig.IsPurchaseAccountingAffected)
                                {
                                    // Stock / Purchase Ledger (Debit)
                                    journalDetails.Add(new JournalEntryDetails(
                                        Guid.NewGuid().ToString(),
                                        newJournalId,
                                        bsConfig.IsPurchaseAmountAdjusted ? LedgerConstants.StockLedgerId : bsConfig.PurchaseAdjustedLedgerId,
                                        bsConfig.IsPurchaseAmountAdjusted ? bsResponse.CalculatedAmount : 0,
                                        bsConfig.IsPurchaseAmountAdjusted ? 0 : bsResponse.CalculatedAmount,
                                        entryDate,
                                        schoolId,
                                        FyId,
                                        true
                                    ));
                                    billSundryDebitTotal += bsConfig.IsPurchaseAmountAdjusted ? bsResponse.CalculatedAmount : 0;
                                    billSundryCreditTotal += bsConfig.IsPurchaseAmountAdjusted ? 0 : bsResponse.CalculatedAmount;

                                    // Vendor Ledger (Credit)
                                    journalDetails.Add(new JournalEntryDetails(
                                        Guid.NewGuid().ToString(),
                                        newJournalId,
                                        bsConfig.VendorAmountAdjusted ? request.ledgerId : bsConfig.VendorAdjustedLedgerId,
                                        bsConfig.VendorAmountAdjusted ? 0 : bsResponse.CalculatedAmount,
                                        bsConfig.VendorAmountAdjusted ? bsResponse.CalculatedAmount : 0,
                                        entryDate,
                                        schoolId,
                                        FyId,
                                        true
                                    ));
                                    billSundryDebitTotal += bsConfig.VendorAmountAdjusted ? 0 : bsResponse.CalculatedAmount;
                                    billSundryCreditTotal += bsConfig.VendorAmountAdjusted ? bsResponse.CalculatedAmount : 0;
                                }
                            }
                        }

                        // ------------------- STOCK / PURCHASE LEDGER -------------------
                        decimal stockDebit = subTotal + billSundryDebitTotal;
                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            LedgerConstants.StockLedgerId,
                            stockDebit,
                            0,
                            entryDate,
                            schoolId,
                            _fiscalContext.CurrentFiscalYearId,
                            true
                        ));

                        // ------------------- VAT LEDGER -------------------
                        if (vatAmount > 0)
                        {
                            journalDetails.Add(new JournalEntryDetails(
                                Guid.NewGuid().ToString(),
                                newJournalId,
                                LedgerConstants.VATLedgerId,
                                vatAmount,
                                0,
                                entryDate,
                                schoolId,
                                _fiscalContext.CurrentFiscalYearId,
                                true
                            ));
                        }

                        // ------------------- DISCOUNT LEDGER -------------------
                        if (discountAmount > 0)
                        {
                            journalDetails.Add(new JournalEntryDetails(
                                Guid.NewGuid().ToString(),
                                newJournalId,
                                LedgerConstants.DiscountLedgerId,
                                0,
                                discountAmount,
                                entryDate,
                                schoolId,
                                _fiscalContext.CurrentFiscalYearId,
                                true
                            ));
                        }

                        // ------------------- VENDOR / CASH / BANK LEDGER -------------------
                        string ledgerIdToUse = null;
                        var hasLedger = !string.IsNullOrEmpty(request.ledgerId);
                        var hasPayment = !string.IsNullOrEmpty(request.paymentId);
                        var specialPaymentId = "d1e43e64-9d48-4c85-83f3-3d2d5c9f6c44";
                        var isSpecial = request.paymentId == specialPaymentId;

                        if (isSpecial && hasLedger)
                            ledgerIdToUse = request.ledgerId;
                        else if (hasPayment)
                        {
                            var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>().GetByGuIdAsync(request.paymentId);
                            var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                                .GetByGuIdAsync(paymentMethod.SubLedgerGroupsId)
                                ?? throw new Exception("Ledger Group not found");

                            var ledger = await _unitOfWork.BaseRepository<Ledger>()
                                .FirstOrDefaultAsync(x => x.SubLedgerGroupId == subledgerGroup.Id);

                            ledgerIdToUse = subledgerGroup.Id switch
                            {
                                SubLedgerGroupConstants.CashInHands => ledger?.Id ?? LedgerConstants.CashLedgerId,
                                SubLedgerGroupConstants.BankAccounts => ledger?.Id ?? LedgerConstants.BankLedgerId,
                                _ => ledger?.Id ?? request.ledgerId
                            };
                        }
                        else if (hasLedger)
                            ledgerIdToUse = request.ledgerId;


                        decimal vendorCredit = 0;
                        // Calculate balancing vendor credit
                        if (!string.IsNullOrEmpty(ledgerIdToUse))
                        {
                            decimal totalDebitSoFar = journalDetails.Sum(x => x.DebitAmount);
                            decimal totalCreditSoFar = journalDetails.Sum(x => x.CreditAmount);
                             vendorCredit = totalDebitSoFar - totalCreditSoFar; // Always positive now

                            journalDetails.Add(new JournalEntryDetails(
                                Guid.NewGuid().ToString(),
                                newJournalId,
                                ledgerIdToUse,
                                0,
                                vendorCredit,
                                entryDate,
                                schoolId,
                                _fiscalContext.CurrentFiscalYearId,
                                true
                            ));

                          
                        }

                        // ------------------- CREATE JOURNAL -------------------
                        var journalData = new JournalEntry(
                            newJournalId,
                            "Purchase Voucher",
                            entryDate,
                            "Being Items Purchased",
                            userId,
                            schoolId,
                            DateTime.UtcNow,
                            "",
                            default,
                            billNumber,
                            FyId,
                            true,
                            journalDetails
                        );

                        // ------------------- VALIDATE BALANCE -------------------
                        decimal totalDebitFinal = journalDetails.Sum(x => x.DebitAmount);
                        decimal totalCreditFinal = journalDetails.Sum(x => x.CreditAmount);

                        if (totalDebitFinal != totalCreditFinal)
                            throw new InvalidOperationException($"Journal unbalanced! Debit={totalDebitFinal}, Credit={totalCreditFinal}");

                        // ------------------- SAVE -------------------
                        await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);



                        string nepaliDate = await _dateConverterHelper.ConvertToNepali(entryDate);

                        var purchaseDetailsData = new PurchaseDetails
                            (
                                    newId,
                                    nepaliDate,
                                    billNumber,
                                    request.ledgerId,
                                    request.amountInWords,
                                    request.discountPercent,
                                    request.discountAmount,
                                    request.vatPercent,
                                    request.vatAmount,
                                    schoolId,
                                    request.grandTotalAmount,
                                    userId,
                                    DateTime.UtcNow,
                                    "",
                                   default,
                                   PurchaseStatus.Settled,
                                   referenceNumber,
                                   newJournalId,
                                   false,
                                   request.paymentId,
                                   request.stockCenterId,
                                   request.isPurchase,
                                request.PurchaseItems?.Select(d => new PurchaseItems
                                      (
                                          Guid.NewGuid().ToString(),
                                          d.quantity,
                                          d.unitId,
                                          d.itemId,
                                          d.price,
                                          d.amount,
                                          userId,
                                          DateTime.Now.ToString(),
                                          "",
                                          "",
                                          false,
                                          newId

                                      )).ToList() ?? new List<PurchaseItems>().ToList()
                            );



                        var inventoryItems = new List<Inventories>();
                        var stockUpdateEvents = new List<StockUpdatedEvent>();
                        foreach (var purchaseItem in request.PurchaseItems)
                        {
                            var purchaseItemsId = purchaseDetailsData.PurchaseItems
                            .FirstOrDefault(x => x.ItemId == purchaseItem.itemId)?.Id;
                            var inventory = new Inventories(
                                Guid.NewGuid().ToString(),
                                purchaseItem.itemId,
                                purchaseItem.quantity,
                                purchaseItem.price,
                                //purchaseItem.price * purchaseItem.quantity,
                                entryDate,
                                0,
                                0,
                                request.ledgerId,
                                false,
                                InventoriesType.Purchase,
                                schoolId,
                                purchaseItem.unitId,
                                purchaseItemsId,
                                null,
                                request.stockCenterId,
                                    userId,
                                DateTime.Now,
                                "",
                                default
                            );
                            inventoryItems.Add(inventory);

                            stockUpdateEvents.Add(new StockUpdatedEvent(purchaseItem.itemId, Convert.ToDouble(purchaseItem.quantity)));
                        }


                        //For Serial Number

                        var itemInstances = new List<ItemInstance>();

                        foreach (var purchaseItem in request.PurchaseItems)
                        {
                            if (purchaseItem.serialNumbers == null || purchaseItem.serialNumbers.Count == 0)
                                continue;

                            var matchedSavedItem = purchaseDetailsData.PurchaseItems
                                .FirstOrDefault(x => x.ItemId == purchaseItem.itemId);

                            if (matchedSavedItem == null)
                                continue;

                            int instanceCount = (int)Math.Min(purchaseItem.quantity, purchaseItem.serialNumbers.Count);

                            for (int i = 0; i < instanceCount; i++)
                            {
                                var itemInstance = new ItemInstance
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ItemsId = purchaseItem.itemId,
                                    SerialNumber = purchaseItem.serialNumbers[i],
                                    PurchaseItemsId = matchedSavedItem.Id,
                                    SalesItemsId = null,
                                    Date = DateTime.UtcNow
                                };

                                itemInstances.Add(itemInstance);
                            }
                        }

                        await _unitOfWork.BaseRepository<Inventories>().AddRange(inventoryItems);


                  



                        await _unitOfWork.BaseRepository<PurchaseDetails>().AddAsync(purchaseDetailsData);
                        await _unitOfWork.BaseRepository<ItemInstance>().AddRange(itemInstances);

                        #region Payment Added
                        PaymentsDetails payment;

                        var paymentMethods = await _unitOfWork.BaseRepository<PaymentMethod>()
                           .GetByGuIdAsync(request.paymentId);

                        if (paymentMethods.SubLedgerGroupsId == SubLedgerGroupConstants.BankAccounts)
                        {
                            payment = new ChequePayment(
                                id: Guid.NewGuid().ToString(),
                                transactionType: TransactionType.Purchase,
                                transactionDate: DateTime.Now,
                                totalAmount: vendorCredit,
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
                                transactionType: TransactionType.Purchase,
                                transactionDate: DateTime.Now,
                                totalAmount: vendorCredit,
                                transactionDetailsId: newId,
                                paymentMethodId: request.paymentId,
                                schoolId: schoolId
                            );
                        }

                        await _unitOfWork.BaseRepository<PaymentsDetails>().AddAsync(payment);



                        #region Delete or Soft Delete Quotation if converted to sales

                        if (!string.IsNullOrEmpty(request.purchaseQuotationNumber))
                        {
                            var quotationData = await _unitOfWork.BaseRepository<PurchaseQuotationDetails>()
                       .FirstOrDefaultAsync(x => x.PurchaseQuotationNumber == request.purchaseQuotationNumber);
                            quotationData.QuotationStatuss = QuotationStatus.Converted;

                            _unitOfWork.BaseRepository<PurchaseQuotationDetails>().Update(quotationData);
                        }



                        #endregion


                        #endregion
                        resultDTOs = _mapper.Map<AddPurchaseDetailsResponse>(purchaseDetailsData);
                   

                    }
                    else
                    {

                        string purchaseQuotationNumber = "";
                        if (schoolSettings.PurchaseQuotationNumberType == PurchaseSalesQuotationNumberType.Manual)
                        {
                            purchaseQuotationNumber = request.purchaseQuotationNumber!;
                        }
                        else
                        {
                            purchaseQuotationNumber = await _billNumberGenerator.GenerateTransactionNumber(schoolId, "PURCHASEQUOTATION", fiscalYear.Data.fyName);
                        }



                        bool purchaseQuotationNumberExists = await _unitOfWork.BaseRepository<PurchaseQuotationDetails>()
                        .AnyAsync(p => p.PurchaseQuotationNumber == purchaseQuotationNumber && p.SchoolId == schoolId);



                        if (purchaseQuotationNumberExists)
                        {
                            return Result<AddPurchaseDetailsResponse>.Failure($"PurchaseQuotation number '{purchaseQuotationNumber}' already exists for this company.");
                        }

                        string nepaliDate = await _dateConverterHelper.ConvertToNepali(entryDate);


                        var purchaseQuotationDetailsData = new PurchaseQuotationDetails
                (
                        newId,
                        nepaliDate,
                        request.billNumber,
                        request.ledgerId,
                        request.amountInWords,
                        request.discountPercent,
                        request.discountAmount,
                        request.vatPercent,
                        request.vatAmount,
                        schoolId,
                        request.grandTotalAmount,
                        userId,
                        DateTime.UtcNow,
                        "",
                       default,
                       referenceNumber,
                       false,
                       request.stockCenterId,
                       purchaseQuotationNumber,
                         request.subTotalAmount,
                                request.taxableAmount,
                                request.amountAfterVat,
                        QuotationStatus.Pending,
                    request.PurchaseItems?.Select(d => new PurchaseQuotationItems
                      (
                          Guid.NewGuid().ToString(),
                          d.quantity,
                          d.unitId,
                          d.itemId,
                          d.price,
                          d.amount,
                          userId,
                          DateTime.Now.ToString(),
                          "",
                          "",
                          false,
                          newId

                      )).ToList() ?? new List<PurchaseQuotationItems>().ToList()
                );
                        await _unitOfWork.BaseRepository<PurchaseQuotationDetails>().AddAsync(purchaseQuotationDetailsData);
                        resultDTOs = _mapper.Map<AddPurchaseDetailsResponse>(purchaseQuotationDetailsData);

                    }
                    await _unitOfWork.SaveChangesAsync();

                    scope.Complete();

                 

                    return Result<AddPurchaseDetailsResponse>.Success(resultDTOs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    if (ex is AggregateException aggEx)
                    {
                        foreach (var inner in aggEx.InnerExceptions)
                        {
                            Console.WriteLine($"Inner Exception: {inner.Message}");
                        }
                    }
                    scope.Dispose();
                    throw new Exception("An error occurred whArgumentException: TN.Purchase.Application.Purchase.Command.AddPurchaseItems.AddPurchaseItemsRequest needs to have a constructor with 0 args or only optional args. Validate your configuration for details. (Parameter 'type')ile adding purchaseDetails", ex);
                }
            }
        }
        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _tokenService.GetUserId();
                var purchaseDetailsList = await _unitOfWork.BaseRepository<PurchaseDetails>()
                    .GetConditionalAsync(
                        x => x.Id == id,
                        query => query
                            .Include(x => x.PurchaseItems)
                                .ThenInclude(x => x.ItemInstances)
                    );

                var singlePurchaseDetail = purchaseDetailsList.FirstOrDefault();
                if (singlePurchaseDetail is null)
                {
                    return Result<bool>.Failure("NotFound", "PurchaseDetails cannot be found");
                }


                var purchaseItems = purchaseDetailsList
                .SelectMany(pd => pd.PurchaseItems)
                .ToList();

                foreach (var item in purchaseItems)
                {
                    var inventoriesToDelete = await _unitOfWork.BaseRepository<Inventories>()
                        .GetConditionalAsync(
                            inv => inv.ItemId == item.ItemId && inv.PurchaseItemsId == item.Id
                        );

                    foreach (var inventory in inventoriesToDelete)
                    {
                        _unitOfWork.BaseRepository<Inventories>().Delete(inventory);
                    }
                }

                var originalJournals = await _unitOfWork.BaseRepository<JournalEntry>()
                    .GetConditionalAsync(x => x.Id == singlePurchaseDetail.JournalEntriesId,
                    query => query.Include(j => j.JournalEntryDetails)
                    );

                var originalJournal = originalJournals.FirstOrDefault();

                #region We should follow this for deletion of journal instead of Hard Delete however Pankaj sir said to go with hard delete for now.

                //if (originalJournal != null)
                //{
                //    var reverseJournalId = Guid.NewGuid().ToString();
                //    var entryDate = DateTime.Now;

                //    // Create reversal details
                //    var reversedDetails = originalJournal.JournalEntryDetails
                //        .Select(d => new JournalEntryDetails(
                //            Guid.NewGuid().ToString(),
                //            reverseJournalId,
                //            d.LedgerId,
                //             d.CreditAmount,   // Swap
                //             d.DebitAmount,   // Swap
                //            entryDate,
                //            originalJournal.CompanyId,
                //            d.FiscalId
                //        ))
                //        .ToList();

                //    var reverseJournal = new JournalEntry(
                //        reverseJournalId,
                //        "Reversal - Purchase Vouchers",
                //        entryDate,
                //        $"Reversal of Journal: {originalJournal.Id}",
                //        currentUserId,
                //        originalJournal.CompanyId,
                //        default,
                //       currentUserId,
                //        DateTime.Now,
                //        originalJournal.BillNumbers,
                //        originalJournal.FyId,
                //        reversedDetails
                //    );

                //    if (reversedDetails.Sum(x => x.DebitAmount) != reversedDetails.Sum(x => x.CreditAmount))
                //        throw new InvalidOperationException("Reversal journal entry is unbalanced.");

                //    await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(reverseJournal);


                //}

                #endregion

                _unitOfWork.BaseRepository<JournalEntry>().Delete(originalJournal);
                _unitOfWork.BaseRepository<PurchaseDetails>().Delete(singlePurchaseDetail);
                await _unitOfWork.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting purchase Details having {id}", ex);
            }
        }
        public async Task<Result<PagedResult<GetAllPurchaseDetailsQueryResponse>>> GetAllPurchaseDetails(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var purchaseDetails = await _unitOfWork.BaseRepository<PurchaseDetails>()
                     .GetConditionalAsync(
                         x=>x.Status != PurchaseStatus.Returned,
                         query => query
                             .Include(p => p.PurchaseItems.Where(pi => !pi.IsDeleted))
                                 .ThenInclude(pi => pi.ItemInstances)
                             .Include(p => p.PurchaseItems.Where(pi=> !pi.IsDeleted))
                                 .ThenInclude(pi => pi.Item)
                     ) ?? new List<PurchaseDetails>();

                                var purchaseDetailsResponses = purchaseDetails.Select(purchase => new GetAllPurchaseDetailsQueryResponse(
                                    purchase.Id,
                                    purchase.Date,
                                    purchase.BillNumber,
                                    purchase.LedgerId,
                                    purchase.AmountInWords,
                                    purchase.DiscountPercent ?? 0,
                                    purchase.DiscountAmount ?? 0,
                                    purchase.VatPercent ?? 0,
                                    purchase.VatAmount ?? 0,
                                    purchase.SchoolId,
                                    purchase.GrandTotalAmount,
                                    purchase.Status,
                                    purchase.ReferenceNumber,
                                    purchase.PaymentId,
                                    purchase.StockCenterId,
                                    purchase.PurchaseItems?.Select(items => new PurchaseItemsDto(
                                        items.Id,
                                        items.Quantity,
                                        items.UnitId,
                                        items.ItemId,
                                        items.Price,
                                        items.Amount,
                                        items.CreatedBy,
                                        items.CreatedAt,
                                        items.UpdatedBy,
                                        items.UpdatedAt,
                                        items.Item?.HsCode ?? "",
                                        items.Item.IsVatEnables,
                                        items.ItemInstances?
                                              .Where(x => !string.IsNullOrWhiteSpace(x.SerialNumber))
                                              .Select(x => x.SerialNumber)
                                              .ToList() ?? new List<string>()
                                    )).ToList() ?? new List<PurchaseItemsDto>()
                                )).ToList();



                var institutionId = _tokenService.InstitutionId() ?? string.Empty;

                var isSuperAdmin = _tokenService.GetRole() == Role.SuperAdmin;


                var filterPurchaseDetails = isSuperAdmin ? purchaseDetailsResponses : purchaseDetailsResponses.Where(x => x.schoolId == _tokenService.SchoolId().FirstOrDefault());

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                   .GetConditionalFilterType(
                       x => x.InstitutionId == institutionId,
                       query => query.Select(c => c.Id)
                   );

                if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(_tokenService.SchoolId().FirstOrDefault()))
                {
                    var filteredEntries = await _unitOfWork.BaseRepository<PurchaseDetails>()
                        .GetConditionalAsync(x => schoolIds.Contains(x.SchoolId) && x.Status != PurchaseStatus.Returned,
                        query => query.Include(j => j.PurchaseItems)
                        .ThenInclude(x => x.ItemInstances)
                        );


                    filterPurchaseDetails = filteredEntries.Select(purchase => new GetAllPurchaseDetailsQueryResponse(
                        purchase.Id,
                    purchase.Date,
                    purchase.BillNumber,
                    purchase.LedgerId,
                    purchase.AmountInWords,
                    purchase.DiscountPercent ?? 0,
                    purchase.DiscountAmount ?? 0,
                    purchase.VatPercent ?? 0,
                    purchase.VatAmount ?? 0,
                    purchase.SchoolId,
                    purchase.GrandTotalAmount,
                    purchase.Status,
                    purchase.ReferenceNumber,
                    purchase.PaymentId,
                    purchase.StockCenterId,
                    purchase.PurchaseItems?.Select(items => new PurchaseItemsDto(
                        items.Id,
                        items.Quantity,
                        items.UnitId,
                        items.ItemId,
                        items.Price,
                        items.Amount,
                        items.CreatedBy,
                        items.CreatedAt,
                        items.UpdatedBy,
                        items.UpdatedAt,
                        items.Item?.HsCode ?? "",
                        items.Item.IsVatEnables,
                        items.ItemInstances?
                              .Where(x => !string.IsNullOrWhiteSpace(x.SerialNumber))
                              .Select(x => x.SerialNumber)
                              .ToList() ?? new List<string>()
                    )).ToList() ?? new List<PurchaseItemsDto>()
                        )).ToList();
                }

                var totalItems = filterPurchaseDetails.Count();

                var paginatedJournalEntries = paginationRequest != null && paginationRequest.IsPagination
                    ? filterPurchaseDetails
                        .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                        .Take(paginationRequest.pageSize)
                        .ToList()
                    : filterPurchaseDetails.ToList();

                var pagedResult = new PagedResult<GetAllPurchaseDetailsQueryResponse>
                {
                    Items = paginatedJournalEntries,
                    TotalItems = totalItems,
                    PageIndex = paginationRequest?.pageIndex ?? 1,
                    pageSize = paginationRequest?.pageSize ?? totalItems
                };


                return Result<PagedResult<GetAllPurchaseDetailsQueryResponse>>.Success(pagedResult);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all journal entry details", ex);
            }
        }
        public async Task<Result<PagedResult<PurchaseReturnDetailsQueryResponse>>> GetAllPurchaseReturnDetails(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var purchaseReturnDetails = await _unitOfWork.BaseRepository<PurchaseReturnDetails>()
                       .GetConditionalAsync(
                         null,
                       query => query.Include(rm => rm.PurchaseReturnItems)
                       ) ?? new List<PurchaseReturnDetails>();


                var purchaseReturnDetailsResponses = purchaseReturnDetails.Select(purchaseReturn => new PurchaseReturnDetailsQueryResponse(
                    purchaseReturn.Id,
                    purchaseReturn.PurchaseDetailsId,
                    purchaseReturn.ReturnDate,
                    purchaseReturn.TotalReturnAmount,
                    purchaseReturn.TaxAdjustment,
                    purchaseReturn.Discount,
                    purchaseReturn.NetReturnAmount,
                    purchaseReturn.SchoolId,
                    purchaseReturn.LedgerId,
                    purchaseReturn.StockCenterId,
                    purchaseReturn.PurchaseReturnItems?.Select(items => new PurchaseReturnItemsDto(
                        items.Id,
                        items.PurchaseReturnDetailsId,
                        items.PurchaseItemsId,
                        items.ReturnQuantity,
                        items.ReturnUnitPrice,
                        items.ReturnTotalAmount
                    )).ToList() ?? new List<PurchaseReturnItemsDto>()
                )).ToList();


                var institutionId = _tokenService.InstitutionId() ?? string.Empty;

                var isSuperAdmin = _tokenService.GetRole() == Role.SuperAdmin;


                var filterPurchaseReturnDetails = isSuperAdmin ? purchaseReturnDetailsResponses : purchaseReturnDetailsResponses.Where(x => x.schoolId == _tokenService.SchoolId().FirstOrDefault());

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                   .GetConditionalFilterType(
                       x => x.InstitutionId == institutionId,
                       query => query.Select(c => c.Id)
                   );

                if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(_tokenService.SchoolId().FirstOrDefault()))
                {
                    var filteredPurchaseReturnEntries = await _unitOfWork.BaseRepository<PurchaseReturnDetails>()
                        .GetConditionalAsync(x => schoolIds.Contains(x.SchoolId),
                        query => query.Include(j => j.PurchaseReturnItems)
                        );

                    filterPurchaseReturnDetails = filteredPurchaseReturnEntries
                        .Select(purchaseReturn => new PurchaseReturnDetailsQueryResponse(
                            purchaseReturn.Id,
                            purchaseReturn.PurchaseDetailsId,
                            purchaseReturn.ReturnDate,
                            purchaseReturn.TotalReturnAmount,
                            purchaseReturn.TaxAdjustment,
                            purchaseReturn.Discount,
                            purchaseReturn.NetReturnAmount,
                            purchaseReturn.SchoolId,
                            purchaseReturn.LedgerId,
                            purchaseReturn.StockCenterId,
                            purchaseReturn.PurchaseReturnItems?.Select(item => new PurchaseReturnItemsDto(
                                item.Id,
                                item.PurchaseReturnDetailsId,
                                item.PurchaseItemsId,
                                item.ReturnQuantity,
                                item.ReturnUnitPrice,
                                item.ReturnTotalAmount
                            )).ToList() ?? new List<PurchaseReturnItemsDto>()
                        )).ToList();

                }




                var totalItems = filterPurchaseReturnDetails.Count();

                var paginatedPurchaseReturnDetailsEntries = paginationRequest != null && paginationRequest.IsPagination
                    ? filterPurchaseReturnDetails
                        .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                        .Take(paginationRequest.pageSize)
                        .ToList()
                    : filterPurchaseReturnDetails.ToList();

                var pagedResult = new PagedResult<PurchaseReturnDetailsQueryResponse>
                {
                    Items = paginatedPurchaseReturnDetailsEntries,
                    TotalItems = totalItems,
                    PageIndex = paginationRequest?.pageIndex ?? 1,
                    pageSize = paginationRequest?.pageSize ?? totalItems
                };


                return Result<PagedResult<PurchaseReturnDetailsQueryResponse>>.Success(pagedResult);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all journal entry details", ex);
            }
        }
        public async Task<Result<BillNumberGenerationBySchoolQueryResponse>> GetBillNumberStatusByCompany(string id, CancellationToken cancellationToken)
        {
            try
            {
                var schoolDetails = await _unitOfWork.BaseRepository<School>().GetByGuIdAsync(id);

                if (schoolDetails == null)
                {
                    return Result<BillNumberGenerationBySchoolQueryResponse>.Failure("School not found.");
                }

                var response = new BillNumberGenerationBySchoolQueryResponse(
                    schoolDetails.BillNumberGenerationTypeForPurchase,
                    id
                );

                return Result<BillNumberGenerationBySchoolQueryResponse>.Success(response);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occured whilw Getting Status by School{id}");
            }
        }
        public async Task<Result<GetPurchaseDetailsByIdQueryResponse>> GetPurchaseDetailsById(string id, CancellationToken cancellationToken = default)
        {

            try
            {

                var purchaseDetails = await _unitOfWork.BaseRepository<PurchaseDetails>()
                     .GetConditionalAsync(
                         x => x.Id == id,
                         query => query
                             .Include(pd => pd.PurchaseItems)
                                 .ThenInclude(pi => pi.Item)               // Include Item
                                     .ThenInclude(i => i.ItemInstances)   // Then include ItemInstances
                     );
                var purchasePaymentDetails = await _unitOfWork.BaseRepository<PaymentsDetails>()
                .FirstOrDefaultAsync(x => x.TransactionDetailsId == id && x.TransactionType == TransactionType.Purchase);



                var purchase = purchaseDetails.FirstOrDefault();
                var purchaseDetailsResponse = new GetPurchaseDetailsByIdQueryResponse(
                    purchase.Id,
                    purchase.Date,
                    purchase.BillNumber,
                    purchase.LedgerId,
                    purchase.AmountInWords,
                    purchase.DiscountPercent,
                    purchase.DiscountAmount,
                    purchase.VatPercent,
                    purchase.VatAmount,
                    purchase.SchoolId,
                   purchase.GrandTotalAmount,
                   purchase.Status,
                   purchase.ReferenceNumber,
                   purchase.PaymentId,
                   purchase.StockCenterId,
                   (purchasePaymentDetails as ChequePayment)?.ChequeNumber,
                    (purchasePaymentDetails as ChequePayment)?.BankName,
                   (purchasePaymentDetails as ChequePayment)?.AccountName,
                    purchase.PurchaseItems?.Select(detail => new PurchaseItemsDto(
                        detail.Id,
                        detail.Quantity,
                        detail.UnitId,
                        detail.ItemId,
                        detail.Price,
                        detail.Amount,
                        detail.CreatedBy,
                        detail.CreatedAt,
                        detail.UpdatedBy,
                        detail.UpdatedAt,
                        detail.Item?.HsCode ?? "",
                        detail.Item.IsVatEnables,
                        detail.ItemInstances?.Select(x => x.SerialNumber).ToList() ?? null
                    )).ToList() ?? new List<PurchaseItemsDto>()
                );


                return Result<GetPurchaseDetailsByIdQueryResponse>.Success(purchaseDetailsResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching purchase details by id", ex);

            }
        }
        public async Task<Result<PurchaseReturnDetailsByIdQueryResponse>> GetPurchaseReturnDetailsById(string id, CancellationToken cancellationToken = default)
        {

            try
            {

                var purchaseReturnDetails = await _unitOfWork.BaseRepository<PurchaseReturnDetails>().
                    GetConditionalAsync(x => x.Id == id,
                    query => query.Include(rm => rm.PurchaseReturnItems)
                    );


                var paymentReturn = await _unitOfWork.BaseRepository<ChequePayment>()
                    .GetConditionalAsync(x => x.TransactionDetailsId == id && x.TransactionType == TransactionType.PurchaseReturn);

                var paymentReturnDetails = paymentReturn.FirstOrDefault();

                var purchaseReturn = purchaseReturnDetails.FirstOrDefault();
                var purchaseReturnDetailsResponse = new PurchaseReturnDetailsByIdQueryResponse(
                    purchaseReturn.Id,
                    purchaseReturn.PurchaseDetailsId,
                    purchaseReturn.ReturnDate,
                    purchaseReturn.TotalReturnAmount,
                    purchaseReturn.TaxAdjustment,
                    purchaseReturn.Discount,
                    purchaseReturn.NetReturnAmount,
                    purchaseReturn.SchoolId,
                    purchaseReturn.StockCenterId,
                    paymentReturnDetails.ChequeNumber,
                    paymentReturnDetails.BankName,
                    paymentReturnDetails.AccountName,
                    purchaseReturn.PurchaseReturnItems?.Select(detail => new PurchaseReturnItemsDto(
                        detail.Id,
                        detail.PurchaseReturnDetailsId,
                        detail.PurchaseItemsId,
                        detail.ReturnQuantity,
                        detail.ReturnUnitPrice,
                        detail.ReturnTotalAmount
                    )).ToList() ?? new List<PurchaseReturnItemsDto>()
                );


                return Result<PurchaseReturnDetailsByIdQueryResponse>.Success(purchaseReturnDetailsResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching purchaseReturn details by id", ex);

            }
        }
        public async Task<Result<PagedResult<FilterPurchaseDetailsByDateQueryResponse>>> GetPurchaseDetailsFilter(PaginationRequest paginationRequest,FilterPurchaseDetailsDTOs filterPurchaseDetailsDTOs)
        {
            try
            {
                var (purchaseDetails, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<PurchaseDetails>();


                var filterItems = isSuperAdmin
                    ? purchaseDetails
                    : purchaseDetails.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                DateTime? startDateUtc = null;
                DateTime? endDateUtc = null;


                if (!string.IsNullOrWhiteSpace(filterPurchaseDetailsDTOs.startDate?.Trim()))
                {
                    startDateUtc = await _dateConverterHelper.ConvertToEnglish(filterPurchaseDetailsDTOs.startDate.Trim());

                    if (DateTime.TryParse(filterPurchaseDetailsDTOs.startDate, out var tempStart) &&
                        tempStart.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = startDateUtc?.AddDays(1);
                    }
                }

                if (!string.IsNullOrWhiteSpace(filterPurchaseDetailsDTOs.endDate?.Trim()))
                {
                    endDateUtc = await _dateConverterHelper.ConvertToEnglish(filterPurchaseDetailsDTOs.endDate.Trim());

                    if (DateTime.TryParse(filterPurchaseDetailsDTOs.endDate, out var tempEnd) &&
                        tempEnd.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = endDateUtc?.AddDays(1);
                    }
                }

                // Handle invalid or missing values
                bool isStartInvalid = !startDateUtc.HasValue || startDateUtc == DateTime.MinValue;
                bool isEndInvalid = !endDateUtc.HasValue || endDateUtc == DateTime.MinValue;

                if (isStartInvalid && isEndInvalid)
                {
                    var todayAtNoon = DateTime.UtcNow;
                    startDateUtc = todayAtNoon.AddDays(-1);
                    endDateUtc = todayAtNoon.Date.AddHours(12);
                }
                else if (!isStartInvalid && isEndInvalid)
                {
                    endDateUtc = startDateUtc.Value.AddHours(1);
                }
                else if (isStartInvalid && !isEndInvalid)
                {
                    startDateUtc = endDateUtc.Value.AddHours(-1);
                }


                var userId = _tokenService.GetUserId;
                var purchaseDetailsResult = await _unitOfWork.BaseRepository<PurchaseDetails>()
                    .GetConditionalAsync(
                        predicate: x =>
                            x.CreatedBy == userId() &&
                            (string.IsNullOrEmpty(filterPurchaseDetailsDTOs.ledgerId) || x.LedgerId.ToLower().Contains(filterPurchaseDetailsDTOs.ledgerId.ToLower())) &&
                            (startDateUtc == null || x.CreatedAt >= startDateUtc) &&
                            (endDateUtc == null || x.CreatedAt <= endDateUtc) && x.Status != PurchaseStatus.Returned,
                        queryModifier: q => q
                            .Include(sd => sd.PurchaseItems)
                                .ThenInclude(i => i.Item)
                            .Include(sd => sd.PurchaseItems.Where(pi=> !pi.IsDeleted))
                                .ThenInclude(i => i.ItemInstances)
                           
                    );


                var responseList = purchaseDetailsResult
             .Where(x => !x.IsDeleted)
             .Select(p =>
             {
                 // Calculate taxable amount (sum of all item amounts before VAT/discount)
                 decimal subTotalAmount = p.PurchaseItems?.Sum(i => i.Amount) ?? 0m;

                 decimal? taxableAmount = subTotalAmount - p.DiscountAmount;


                 return new FilterPurchaseDetailsByDateQueryResponse(
                     p.Id,
                     p.Date,
                     p.BillNumber,
                     p.LedgerId,
                     p.AmountInWords,
                     p.DiscountPercent,
                     p.DiscountAmount,
                     p.VatPercent,
                     p.VatAmount,
                     p.SchoolId,
                     p.GrandTotalAmount,
                     p.Status,
                     p.ReferenceNumber,
                     p.StockCenterId,
                          taxableAmount,
                     subTotalAmount,
                     p.PurchaseItems?.Select(i => new PurchaseItemsDto(
                         i.Id,
                         i.Quantity,
                         i.UnitId,
                         i.ItemId,
                         i.Price,
                         i.Amount,
                         i.CreatedBy,
                         i.CreatedAt,
                         i.UpdatedBy,
                         i.UpdatedAt,
                         i.Item?.HsCode ?? "",
                         i.Item?.IsVatEnables ?? false,
                         i.ItemInstances?.Where(x => !string.IsNullOrWhiteSpace(x.SerialNumber))
                                          .Select(x => x.SerialNumber)
                                          .ToList() ?? new List<string>()
                     )).ToList() ?? new List<PurchaseItemsDto>(),
                     p.PurchaseItems?.Select(y => new QuantityDetailDto(
                         y.Id,
                         y.ItemId,
                         y.UnitId,
                         y.Quantity,
                         y.ItemInstances?.Where(s => !string.IsNullOrWhiteSpace(s.SerialNumber))
                                          .Select(s => s.SerialNumber)
                                          .ToList() ?? new List<string?>()
                     )).ToList() ?? new List<QuantityDetailDto>()
                    
                 );
             }).ToList();


                PagedResult<FilterPurchaseDetailsByDateQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterPurchaseDetailsByDateQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterPurchaseDetailsByDateQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }
                return Result<PagedResult<FilterPurchaseDetailsByDateQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching purchaseDetails: {ex.Message}", ex);
            }
        }
        public async Task<Result<UpdatePurchaseDetailsResponse>> Update(string id, UpdatePurchaseDetailsCommand updatePurchaseDetailsCommand)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var schoolId = _tokenService.SchoolId().FirstOrDefault();
                if (schoolId == null)
                    throw new UnauthorizedAccessException("School ID not found in token.");
                if (string.IsNullOrEmpty(id))
                    return Result<UpdatePurchaseDetailsResponse>.Failure("InvalidRequest", "Purchase ID cannot be null or empty");


                var school = await _unitOfWork.BaseRepository<School>().GetByGuIdAsync(schoolId);
                if (school == null)
                {
                    return Result<UpdatePurchaseDetailsResponse>.Failure("Invalid SchoolId. School does not exist.");
                }


                var purchase = (await _unitOfWork.BaseRepository<PurchaseDetails>()
                    .GetConditionalAsync(x => x.Id == id,
                        q => q.Include(p => p.PurchaseItems).ThenInclude(i => i.ItemInstances)))
                    .FirstOrDefault();

                if (purchase is null)
                    return Result<UpdatePurchaseDetailsResponse>.Failure("NotFound", "Purchase not found");

                purchase.SchoolId = schoolId;
                purchase.ModifiedBy = "";
                purchase.ModifiedAt = DateTime.UtcNow;

                if (updatePurchaseDetailsCommand.updatePurchaseItems?.Any() == true)
                {
                    foreach (var item in updatePurchaseDetailsCommand.updatePurchaseItems)
                    {
                        var existingItem = await _unitOfWork.BaseRepository<PurchaseItems>().GetByGuIdAsync(item.id);

                        var inventory = await _unitOfWork.BaseRepository<Inventories>().FirstOrDefaultAsync(
                            x => x.PurchaseItemsId == item.id && x.SchoolId == schoolId && x.ItemId == item.itemId);

                        if (inventory != null)
                        {
                            inventory.QuantityIn = item.quantity;
                            inventory.AmountIn = item.amount;
                            inventory.ItemId = item.itemId;
                            _unitOfWork.BaseRepository<Inventories>().Update(inventory);
                        }

                        if (existingItem != null)
                        {
                            existingItem.Quantity = item.quantity;
                            existingItem.UnitId = item.unitId;
                            existingItem.ItemId = item.itemId;
                            existingItem.Price = Convert.ToDecimal(item.price);
                            existingItem.Amount = item.amount;

                            var instances = existingItem.ItemInstances?.ToList() ?? new List<ItemInstance>();

                            for (int i = 0; i < item.serialNumbers?.Count; i++)
                            {
                                var serial = item.serialNumbers[i];

                                if (string.IsNullOrWhiteSpace(serial))
                                    continue;

                                if (i < instances.Count)
                                    instances[i].SerialNumber = item.serialNumbers[i];
                                else
                                    instances.Add(new ItemInstance
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        PurchaseItemsId = existingItem.Id,
                                        SerialNumber = item.serialNumbers[i],
                                    });
                            }

                            if (instances.Count > item.serialNumbers?.Count)
                                instances.RemoveRange(item.serialNumbers.Count, instances.Count - item.serialNumbers.Count);

                            existingItem.ItemInstances = instances;
                            _unitOfWork.BaseRepository<PurchaseItems>().Update(existingItem);
                        }
                        else
                        {
                            var newItem = _mapper.Map<PurchaseItems>(item);
                            newItem.Id = Guid.NewGuid().ToString();
                            newItem.PurchaseDetailsId = purchase.Id;
                            newItem.CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                            newItem.UpdatedAt = newItem.CreatedAt;
                            newItem.CreatedBy = "";
                            newItem.UpdatedBy = "";
                            await _unitOfWork.BaseRepository<PurchaseItems>().AddAsync(newItem);
                        }
                    }
                    await _unitOfWork.SaveChangesAsync();
                }

                _mapper.Map(updatePurchaseDetailsCommand, purchase);
                await _unitOfWork.SaveChangesAsync();


                var journal = (await _unitOfWork.BaseRepository<JournalEntry>()
                        .GetConditionalAsync(j => j.Id == purchase.JournalEntriesId,
                            q => q.Include(j => j.JournalEntryDetails)))
                        .FirstOrDefault();

                if (journal != null)
                {
                    // Remove old details
                    _unitOfWork.BaseRepository<JournalEntryDetails>().DeleteRange(journal.JournalEntryDetails.ToList());

                    // --- Purchase side amounts ---
                    decimal gross = purchase.PurchaseItems.Sum(x => x.Amount);
                    decimal discount = purchase.DiscountAmount ?? 0;
                    decimal vat = purchase.VatAmount ?? 0;
                    decimal net = gross - discount + vat;

                    DateTime transactionDate = DateTime.TryParse(purchase.Date, out var dt)
                        ? dt
                        : purchase.CreatedAt;

                    string fiscalId = journal.FyId ?? _fiscalContext.CurrentFiscalYearId;

                    var journalDetails = new List<JournalEntryDetails>();

                    // --- Purchase entries ---
                    AddJournal(LedgerConstants.StockLedgerId, gross, 0);

                    if (vat > 0)
                        AddJournal(LedgerConstants.VATLedgerId, vat, 0);

                    if (discount > 0)
                        AddJournal(LedgerConstants.DiscountLedgerId, 0, discount);

                    if (!string.IsNullOrEmpty(purchase.LedgerId))
                        AddJournal(purchase.LedgerId, 0, net);

                    // --- Payment entries ---
                    var specialPaymentId = "d1e43e64-9d48-4c85-83f3-3d2d5c9f6c44";
                    bool hasPayment = !string.IsNullOrEmpty(updatePurchaseDetailsCommand.paymentId);
                    bool hasLedger = !string.IsNullOrEmpty(updatePurchaseDetailsCommand.ledgerId);
                    bool isSpecial = updatePurchaseDetailsCommand.paymentId == specialPaymentId;
                    decimal netTotalAmount = net;

                    if (isSpecial && hasLedger)
                    {
                        AddJournal(updatePurchaseDetailsCommand.ledgerId, 0, netTotalAmount);
                    }
                    else if (hasPayment)
                    {
                        var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>()
                            .GetByGuIdAsync(updatePurchaseDetailsCommand.paymentId);

                        var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                            .GetByGuIdAsync(paymentMethod.SubLedgerGroupsId)
                            ?? throw new Exception("Ledger group not found.");

                        var ledger = await _unitOfWork.BaseRepository<Ledger>()
                            .FirstOrDefaultAsync(x => x.SubLedgerGroupId == subledgerGroup.Id);

                        switch (subledgerGroup.Id)
                        {
                            case SubLedgerGroupConstants.CashInHands:
                                AddJournal(LedgerConstants.CashLedgerId, 0, netTotalAmount);
                                break;

                            case SubLedgerGroupConstants.BankAccounts:
                                AddJournal(ledger?.Id ?? LedgerConstants.BankLedgerId, 0, netTotalAmount);
                                break;

                            default:
                                AddJournal(ledger.Id, 0, netTotalAmount);
                                break;
                        }
                    }

                    if (hasLedger && !isSpecial)
                    {
                        AddJournal(updatePurchaseDetailsCommand.ledgerId, 0, netTotalAmount);
                    }

                    if (hasLedger && hasPayment && !isSpecial)
                    {
                        AddJournal(updatePurchaseDetailsCommand.ledgerId, netTotalAmount, 0);
                    }

                    // Save details
                    await _unitOfWork.BaseRepository<JournalEntryDetails>().AddRange(journalDetails);

                    // Update journal metadata
                    journal.TransactionDate = transactionDate;
                    journal.Description = "Updated Purchase Entry";
                    journal.ModifiedAt = DateTime.UtcNow;
                    journal.ModifiedBy = ""; // TODO: replace with current user

                    _unitOfWork.BaseRepository<JournalEntry>().Update(journal);
                    await _unitOfWork.SaveChangesAsync();


                    // --- Local helper method ---
                    void AddJournal(string ledgerId, decimal debit, decimal credit)
                    {
                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            journal.Id,
                            ledgerId,
                            debit,
                            credit,
                            transactionDate,
                            schoolId,
                            fiscalId,
                            true
                        ));
                    }
                }



                #region Update PaymentDetails

                var paymentDetails = await _unitOfWork.BaseRepository<PaymentsDetails>()
            .FirstOrDefaultAsync(x => x.TransactionDetailsId == id && x.TransactionType == TransactionType.Purchase);

                if (paymentDetails is not null)
                {
                    paymentDetails.TotalAmount = updatePurchaseDetailsCommand.grandTotalAmount;
                    _unitOfWork.BaseRepository<PaymentsDetails>().Update(paymentDetails);
                }

                #endregion

                scope.Complete();

                var result = new UpdatePurchaseDetailsResponse(
                    id,
                purchase.BillNumber,
                purchase.LedgerId,
                purchase.AmountInWords,
                purchase.DiscountPercent,
                purchase.DiscountAmount,
                    purchase.VatPercent,
                    purchase.VatAmount,
                    purchase.GrandTotalAmount,
                    purchase.PaymentId,
                    purchase.ReferenceNumber,
                    purchase.StockCenterId,
                    purchase.PurchaseItems?.Select(x => new UpdatePurchaseItems(
                        x.Quantity, x.UnitId, x.ItemId, x.Price, x.Amount, x.PurchaseDetailsId)).ToList()
                    ?? new List<UpdatePurchaseItems>());

                return Result<UpdatePurchaseDetailsResponse>.Success(result);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the purchaseDetails", ex);
            }

        }

        public async Task<Result<UpdateBillNumberGeneratorBySchoolResponse>> UpdateBillNumberStatusByCompany(UpdateBillNumberGeneratorBySchoolCommand updateBillNumberGeneratorBySchoolCommand)
        {

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (string.IsNullOrEmpty(updateBillNumberGeneratorBySchoolCommand.schoolId))
                    {
                        return Result<UpdateBillNumberGeneratorBySchoolResponse>.Failure("InvalidRequest", "School ID cannot be null or empty");
                    }

                    var school = await _unitOfWork.BaseRepository<School>().GetByGuIdAsync(updateBillNumberGeneratorBySchoolCommand.schoolId);

                    school.BillNumberGenerationTypeForPurchase = updateBillNumberGeneratorBySchoolCommand.BillNumberGenerationType;
                    _unitOfWork.BaseRepository<School>().Update(school);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateBillNumberGeneratorBySchoolResponse
                    (
                            school.BillNumberGenerationTypeForPurchase,
                            school.Id

                    );

                    return Result<UpdateBillNumberGeneratorBySchoolResponse>.Success((UpdateBillNumberGeneratorBySchoolResponse)resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the purchaseDetails", ex);
                }


            }
        }
        public async Task<Result<AddPurchaseReturnDetailsResponse>> AddPurchaseReturn(AddPurchaseReturnDetailsCommand request)
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
                        return Result<AddPurchaseReturnDetailsResponse>.Failure("Invalid SchoolId. School does not exist.");

                    if (request.PurchaseReturnItems == null || !request.PurchaseReturnItems.Any())
                        return Result<AddPurchaseReturnDetailsResponse>.Failure("No return items provided.");

                    if (request.PurchaseReturnItems.Any(x => string.IsNullOrWhiteSpace(x.purchaseItemsId)))
                        return Result<AddPurchaseReturnDetailsResponse>.Failure("One or more PurchaseReturnItems are missing PurchaseItemsId.");

                    DateTime returnedDate = request.returnDate == default
                        ? DateTime.Today
                        : await _dateConverterHelper.ConvertToEnglish(request.returnDate);

                    var purchaseDetails = await _unitOfWork.BaseRepository<PurchaseDetails>().GetByGuIdAsync(request.purchaseDetailsId);
                    if (purchaseDetails == null)
                        throw new Exception("PurchaseDetails not found.");

                    var quantityReturned = request.PurchaseReturnItems.Sum(x => x.returnQuantity);

                    var quantityToBeReturnedItemList = await _unitOfWork.BaseRepository<PurchaseDetails>().GetConditionalAsync(
                        x => x.Id == request.purchaseDetailsId,
                        query => query.Include(x => x.PurchaseItems));

                    var quantityToBeReturned = quantityToBeReturnedItemList
                        .SelectMany(x => x.PurchaseItems)
                        .Sum(x => x.Quantity);

                    var purchaseDetailsToBeUpdated = await _unitOfWork.BaseRepository<PurchaseDetails>().GetByGuIdAsync(request.purchaseDetailsId);





                    var fiscalYear = await _settingServices.GetCurrentFiscalYearBy(schoolId, default);



                    var schoolSettings = await _unitOfWork.BaseRepository<SchoolSettings>().FirstOrDefaultAsync(x => x.SchoolId == schoolId);
                    if (schoolSettings == null)
                    {
                        return Result<AddPurchaseReturnDetailsResponse>.Failure("Invalid SchoolId. Company does not exist.");
                    }
                    string purchaseReturnNumber = "";
                    if (schoolSettings.IncomeTransactionNumberType == TransactionNumberType.Manual)
                    {
                        purchaseReturnNumber = request.purchaseReturnNumber!;
                    }
                    else
                    {
                        purchaseReturnNumber = await _billNumberGenerator.GenerateTransactionNumber(schoolId, "PURCHASERETURN", fiscalYear.Data.fyName);
                    }



                    bool incomeNumberExists = await _unitOfWork.BaseRepository<PurchaseReturnDetails>()
                    .AnyAsync(p => p.PurchaseReturnNumber == purchaseReturnNumber && p.SchoolId == schoolId);



                    if (incomeNumberExists)
                    {
                        return Result<AddPurchaseReturnDetailsResponse>.Failure($"Purchase Return number '{purchaseReturnNumber}' already exists for this company.");
                    }



                    #region Journal Entries

                    string newJournalId = Guid.NewGuid().ToString();

                    decimal grossPurchaseReturnAmount = request.PurchaseReturnItems.Sum(x => x.returnTotalAmount);

                    decimal vatAmount = request.taxAdjustment;

                    decimal discount = request.discount;

                    decimal NetReturnAmount = grossPurchaseReturnAmount - discount + vatAmount;

                    var journalDetails = new List<JournalEntryDetails>
                        {
                            new JournalEntryDetails(
                                Guid.NewGuid().ToString(),
                                newJournalId,
                                LedgerConstants.StockLedgerId,
                                0,
                                grossPurchaseReturnAmount,
                                returnedDate,
                                schoolId,
                                _fiscalContext.CurrentFiscalYearId,
                                true
                            )
                        };

                    if (vatAmount > 0)
                    {
                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            LedgerConstants.VATLedgerId,
                            0,
                            vatAmount,
                            returnedDate,
                            schoolId,
                            _fiscalContext.CurrentFiscalYearId,
                            true
                        ));
                    }

                    if (discount > 0)
                    {
                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            LedgerConstants.DiscountLedgerId,
                            discount,
                            0,
                            returnedDate,
                            schoolId,
                            _fiscalContext.CurrentFiscalYearId,
                            true
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
                                AddJournal(LedgerConstants.CashLedgerId, NetReturnAmount, 0);
                                break;

                            case SubLedgerGroupConstants.BankAccounts:
                                AddJournal(LedgerConstants.BankLedgerId, NetReturnAmount, 0);
                                break;


                            default:
                                AddJournal(ledger.Id, NetReturnAmount, 0);
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
                            _fiscalContext.CurrentFiscalYearId,
                            true
                        ));
                    }



                    var debitAmount = journalDetails.Sum(x => x.DebitAmount);
                    var creditAmount = journalDetails.Sum(x => x.CreditAmount);

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
                           purchaseDetails.BillNumber,
                           FyId,
                           true,
                           journalDetails
                       );

                    await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);
                    #endregion







                    if (quantityReturned == quantityToBeReturned && quantityReturned > 0)
                        purchaseDetailsToBeUpdated.Status = PurchaseStatus.Returned;
                    else if (quantityReturned < quantityToBeReturned && quantityReturned > 0)
                        purchaseDetailsToBeUpdated.Status = PurchaseStatus.PartiallyReturned;

                    _unitOfWork.BaseRepository<PurchaseDetails>().Update(purchaseDetailsToBeUpdated);

                    purchaseDetails.GrandTotalAmount -= request.netReturnAmount;
                    

                    if (purchaseDetails.GrandTotalAmount == 0)
                    {
                        purchaseDetails.IsDeleted = true;
                        _unitOfWork.BaseRepository<PurchaseDetails>().Update(purchaseDetails);

                    }
                        

                    purchaseDetails.VatAmount = purchaseDetails.VatAmount - request.taxAdjustment;
                    purchaseDetails.DiscountAmount = purchaseDetails.DiscountAmount - request.discount;

                    foreach (var detail in request.PurchaseReturnItems)
                    {
                        var existingPurchase = await _unitOfWork.BaseRepository<PurchaseItems>().GetByGuIdAsync(detail.purchaseItemsId);

                        if (existingPurchase != null)
                        {
                            if (detail.returnQuantity <= 0 || detail.returnQuantity > existingPurchase.Quantity)
                                throw new InvalidOperationException("Invalid return quantity.");

                            existingPurchase.Quantity -= detail.returnQuantity;
                            existingPurchase.Price = detail.returnQuantity > 0
                                ? detail.returnTotalAmount / detail.returnQuantity
                                : 0;
                            existingPurchase.Amount -= detail.returnTotalAmount;

                            var itemInstance = await _unitOfWork.BaseRepository<ItemInstance>()
                                .GetConditionalAsync(x => x.PurchaseItemsId == existingPurchase.Id);


                            existingPurchase.IsDeleted = true;

                            if (existingPurchase.Quantity == 0)
                            {
                                if (itemInstance != null && itemInstance.Count() != 0)
                                    _unitOfWork.BaseRepository<ItemInstance>().DeleteRange(itemInstance.ToList());

                                _unitOfWork.BaseRepository<PurchaseItems>().Update(existingPurchase);
                            }
                            else
                            {
                                if (itemInstance != null)
                                    _unitOfWork.BaseRepository<ItemInstance>().DeleteRange(itemInstance.ToList());
                                _unitOfWork.BaseRepository<PurchaseItems>().Update(existingPurchase);
                            }
                        }




                        var inventories = await _unitOfWork.BaseRepository<Inventories>()
                            .FirstOrDefaultAsync(x => x.PurchaseItemsId == detail.purchaseItemsId && x.ItemId == detail.itemsId);

                        if (inventories != null)
                        {
                            inventories.QuantityIn -= detail.returnQuantity;
                            inventories.AmountIn -= detail.returnTotalAmount;

                            if (inventories.QuantityIn == 0)
                                _unitOfWork.BaseRepository<Inventories>().Delete(inventories);
                            else
                                _unitOfWork.BaseRepository<Inventories>().Update(inventories);
                        }
                        else
                        {
                            var inventoryLogs = await _unitOfWork.BaseRepository<InventoriesLogs>()
                                .FirstOrDefaultAsync(x => x.PurchaseItemsId == detail.purchaseItemsId && x.ItemId == detail.itemsId);

                            if (inventoryLogs != null)
                            {
                                inventoryLogs.QuantityIn -= detail.returnQuantity;
                                inventoryLogs.AmountIn -= detail.returnTotalAmount;

                                if (inventoryLogs.QuantityIn == 0)
                                    _unitOfWork.BaseRepository<InventoriesLogs>().Delete(inventoryLogs);
                                else
                                    _unitOfWork.BaseRepository<InventoriesLogs>().Update(inventoryLogs);
                            }
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
                            transactionType: TransactionType.PurchaseReturn,
                            transactionDate: DateTime.Now,
                            totalAmount: NetReturnAmount,
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
                            transactionType: TransactionType.PurchaseReturn,
                            transactionDate: DateTime.Now,
                            totalAmount: NetReturnAmount,
                            transactionDetailsId: newId,
                            paymentMethodId: request.paymentId,
                            schoolId: schoolId
                        );
                    }

                    await _unitOfWork.BaseRepository<PaymentsDetails>().AddAsync(payment);

                    #endregion

                    if (request.PurchaseReturnItems.Any(x => string.IsNullOrWhiteSpace(x.purchaseItemsId)))
                        return Result<AddPurchaseReturnDetailsResponse>.Failure("One or more PurchaseReturnItems are missing PurchaseItemsId.");



                    var purchaseReturnItems = request.PurchaseReturnItems.Select(d => new PurchaseReturnItems(
                        Guid.NewGuid().ToString(),
                        newId,
                        d.purchaseItemsId!,
                        d.returnQuantity,
                        d.returnUnitPrice,
                        d.returnTotalAmount,
                        userId,
                        DateTime.UtcNow.ToString("s"),
                        "",
                        ""
                    )).ToList();

                    var purchaseReturnDetailsData = new PurchaseReturnDetails(
                        newId,
                        request.purchaseDetailsId,
                        returnedDate,
                        request.totalReturnAmount,
                        request.taxAdjustment,
                        request.discount,
                        request.netReturnAmount,
                        schoolId,
                        userId,
                        request.reason,
                        purchaseDetails.LedgerId,
                        purchaseDetails.StockCenterId,
                        DateTime.UtcNow,
                        "",
                        default,
                        newJournalId,
                        purchaseReturnNumber,
                        purchaseReturnItems
                    );
                    await _unitOfWork.BaseRepository<PurchaseReturnDetails>().AddAsync(purchaseReturnDetailsData);


                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();
                    var resultDTO = _mapper.Map<AddPurchaseReturnDetailsResponse>(purchaseReturnDetailsData);
                    return Result<AddPurchaseReturnDetailsResponse>.Success(resultDTO);



                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while returning purchase");

                }
            }
        }
        public async Task<Result<bool>> DeletePurchaseReturnDetails(string id, CancellationToken cancellationToken)
        {
            try
            {

                var purchaseReturnDetails = await _unitOfWork.BaseRepository<PurchaseReturnDetails>().GetByGuIdAsync(id);
                if (purchaseReturnDetails is null)
                {
                    return Result<bool>.Failure("NotFound", "purchaseReturnDetails  Cannot be Found");
                }


                var originalJournals = await _unitOfWork.BaseRepository<JournalEntry>()
              .GetConditionalAsync(x => x.Id == purchaseReturnDetails.JournalEntriesId,
              query => query.Include(j => j.JournalEntryDetails)
              );
                var originalJournal = originalJournals.FirstOrDefault();
                _unitOfWork.BaseRepository<JournalEntry>().Delete(originalJournal);


                _unitOfWork.BaseRepository<PurchaseReturnDetails>().Delete(purchaseReturnDetails);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting purchase Return Details having {id}", ex);
            }
        }

        public async Task<Result<UpdatePurchaseReturnDetailsResponse>> Update(string id, UpdatePurchaseReturnDetailsCommand updatePurchaseReturnDetailsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (string.IsNullOrEmpty(id))
                    {
                        return Result<UpdatePurchaseReturnDetailsResponse>.Failure("InvalidRequest", "PurchaseReturn Details ID cannot be null or empty");
                    }
                    var purchaseReturnDetails = await _unitOfWork.BaseRepository<PurchaseReturnDetails>().GetConditionalAsync(x => x.Id == id,
                            query => query.Include(rm => rm.PurchaseReturnItems));

                    var purchaseReturn = purchaseReturnDetails.FirstOrDefault();
                    if (purchaseReturn == null)
                    {
                        return Result<UpdatePurchaseReturnDetailsResponse>.Failure("NotFound", "purchase Return details not found");
                    }

                    purchaseReturn.SchoolId = _tokenService.SchoolId().FirstOrDefault();

                    purchaseReturn.ModifiedBy = "";
                    purchaseReturn.ModifiedAt = DateTime.UtcNow;


                    if (updatePurchaseReturnDetailsCommand.purchaseReturnItems != null && updatePurchaseReturnDetailsCommand.purchaseReturnItems.Any())
                    {
                        foreach (var detail in updatePurchaseReturnDetailsCommand.purchaseReturnItems)
                        {
                            var existingPurchaseReturn = await _unitOfWork.BaseRepository<PurchaseReturnItems>().GetByGuIdAsync(detail.id);

                            if (existingPurchaseReturn != null)
                            {
                                _mapper.Map(detail, existingPurchaseReturn);
                                _unitOfWork.BaseRepository<PurchaseReturnItems>().Update(existingPurchaseReturn);
                            }
                            else
                            {
                                var newPurchaseReturn = _mapper.Map<PurchaseReturnItems>(detail);
                                newPurchaseReturn.Id = Guid.NewGuid().ToString();
                                await _unitOfWork.BaseRepository<PurchaseReturnItems>().AddAsync(newPurchaseReturn);
                            }
                        }

                        await _unitOfWork.SaveChangesAsync();
                    }

                    _mapper.Map(updatePurchaseReturnDetailsCommand, purchaseReturn);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdatePurchaseReturnDetailsResponse
                    (
                            id,
                            purchaseReturn.PurchaseDetailsId,
                            purchaseReturn.ReturnDate,
                            purchaseReturn.TotalReturnAmount,
                            purchaseReturn.TaxAdjustment,
                            purchaseReturn.Discount,
                            purchaseReturn.NetReturnAmount,
                            "",


                             purchaseReturn.PurchaseReturnItems?.Select(item => new UpdatePurchaseReturnItems
                             (
                                 item.Id,
                                 item.PurchaseReturnDetailsId,
                                 item.PurchaseItemsId,
                                 item.ReturnQuantity,
                                 item.ReturnUnitPrice,
                                 item.ReturnTotalAmount



                             )).ToList() ?? new List<UpdatePurchaseReturnItems>()
                    );

                    return Result<UpdatePurchaseReturnDetailsResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while updating the purchase Return Details", ex);
                }


            }
        }

        public async Task<Result<PagedResult<GetFilterPurchaseReturnDetailsQueryResponse>>> GetFilterPurchaseReturnDetails(PaginationRequest paginationRequest,FilterPurchaseReturnDetailsDtos filterPurchaseReturnDetailsDtos)
        {
            try
            {
                var (ledger, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<PurchaseReturnDetails>();

                var filteredLedger = isSuperAdmin
                    ? ledger
                    : ledger.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                DateTime? startDateUtc = null;
                DateTime? endDateUtc = null;

                // Convert Nepali Start Date
                if (!string.IsNullOrWhiteSpace(filterPurchaseReturnDetailsDtos.startDate?.Trim()))
                {
                    startDateUtc = await _dateConverterHelper.ConvertToEnglish(filterPurchaseReturnDetailsDtos.startDate.Trim());

                    if (DateTime.TryParse(filterPurchaseReturnDetailsDtos.startDate, out var tempStart) &&
                        tempStart.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = startDateUtc?.AddDays(1);
                    }
                }

                // Convert Nepali End Date
                if (!string.IsNullOrWhiteSpace(filterPurchaseReturnDetailsDtos.endDate?.Trim()))
                {
                    endDateUtc = await _dateConverterHelper.ConvertToEnglish(filterPurchaseReturnDetailsDtos.endDate.Trim());

                    if (DateTime.TryParse(filterPurchaseReturnDetailsDtos.endDate, out var tempEnd) &&
                        tempEnd.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = endDateUtc?.AddDays(1);
                    }
                }

                // Handle fallback to today if both are missing
                bool isStartInvalid = !startDateUtc.HasValue || startDateUtc == DateTime.MinValue;
                bool isEndInvalid = !endDateUtc.HasValue || endDateUtc == DateTime.MinValue;

                if (isStartInvalid && isEndInvalid)
                {
                    var todayUtc = DateTime.UtcNow.Date;
                    startDateUtc = todayUtc;
                    endDateUtc = todayUtc.AddDays(1);
                }
                else if (!isStartInvalid && isEndInvalid)
                {
                    endDateUtc = startDateUtc.Value.AddDays(1);
                }
                else if (isStartInvalid && !isEndInvalid)
                {
                    startDateUtc = endDateUtc.Value.AddDays(-1);
                }

                var userId = _tokenService.GetUserId();

                var purchaseReturnDetailsResult = await _unitOfWork.BaseRepository<PurchaseReturnDetails>()
                    .GetConditionalAsync(
                        predicate: x =>
                            x.CreatedBy == userId &&

                            (string.IsNullOrEmpty(filterPurchaseReturnDetailsDtos.ledgerId) ||
                            x.LedgerId == filterPurchaseReturnDetailsDtos.ledgerId) &&
                            (startDateUtc == null || x.CreatedAt >= startDateUtc) &&
                            (endDateUtc == null || x.CreatedAt < endDateUtc),
                            queryModifier: q => q
                            .Include(pr => pr.PurchaseReturnItems)
                                .ThenInclude(pi => pi.PurchaseItems)
                    );

                var responseList = purchaseReturnDetailsResult.Select(pr => {
                    decimal subTotalAmount = pr.PurchaseReturnItems?.Sum(i => i.ReturnTotalAmount) ?? 0m;

                    decimal? taxableAmount = subTotalAmount - pr.Discount;
                    return new GetFilterPurchaseReturnDetailsQueryResponse(
                        pr.Id,
                        pr.PurchaseDetailsId,
                        pr.ReturnDate,
                        pr.TotalReturnAmount,
                        pr.TaxAdjustment,
                        pr.Discount,
                        pr.NetReturnAmount,
                        pr.SchoolId,
                        pr.LedgerId,
                        pr.StockCenterId,
                        taxableAmount,
                        subTotalAmount,
                        pr.PurchaseReturnItems?.Select(i => new PurchaseReturnItemsDto(
                            i.Id,
                            i.PurchaseReturnDetailsId,
                            i.PurchaseItemsId,
                            i.ReturnQuantity,
                            i.ReturnUnitPrice,
                            i.ReturnTotalAmount
                        )).ToList() ?? new List<PurchaseReturnItemsDto>()
                            );
                }).ToList();

                int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;
                int totalItems = responseList.Count;

                var pagedItems = responseList
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var pagedResult = new PagedResult<GetFilterPurchaseReturnDetailsQueryResponse>
                {
                    Items = pagedItems,
                    TotalItems = totalItems,
                    PageIndex = pageIndex,
                    pageSize = pageSize
                };

                return Result<PagedResult<GetFilterPurchaseReturnDetailsQueryResponse>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching purchase return details: {ex.Message}", ex);
            }
        }

        public async Task<Result<GetPurchaseDetailsQueryResponse>> GetPurchaseDetailsByRefNo(string referenceNumber, CancellationToken cancellationToken = default)
        {
            try
            {

                string decodedReferenceNumber = WebUtility.UrlDecode(referenceNumber);

                var purchaseDetailsData = await _unitOfWork.BaseRepository<PurchaseDetails>()
                 .GetConditionalAsync(
                     x => x.ReferenceNumber.Trim() == decodedReferenceNumber.Trim(),
                     query => query.Include(rm => rm.PurchaseItems)
                                   .ThenInclude(x => x.ItemInstances)
                 );

                var purchaseDetail = purchaseDetailsData.FirstOrDefault();

                if (purchaseDetail == null)
                {
                    return Result<GetPurchaseDetailsQueryResponse>.Failure("No purchase details found for the given reference number.");
                }

                var purchaseResponse = new GetPurchaseDetailsQueryResponse(
                    purchaseDetail.Id,
                    purchaseDetail.Date,
                    purchaseDetail.BillNumber,
                    purchaseDetail.LedgerId,
                    purchaseDetail.AmountInWords,
                    purchaseDetail.DiscountPercent ?? 0,
                    purchaseDetail.DiscountAmount ?? 0,
                    purchaseDetail.VatPercent ?? 0,
                    purchaseDetail.VatAmount ?? 0,
                    purchaseDetail.SchoolId,
                    purchaseDetail.GrandTotalAmount,
                    purchaseDetail.ReferenceNumber,
                    purchaseDetail.PaymentId,
                    purchaseDetail.StockCenterId,
                    purchaseDetail.PurchaseItems?.Select(d => new PurchaseItemsDto(
                        d.Id,
                        d.Quantity,
                        d.UnitId,
                        d.ItemId,
                        d.Price,
                        d.Amount,
                        d.CreatedBy,
                        d.CreatedAt,
                        d.UpdatedBy,
                        d.UpdatedAt,
                        d.Item?.HsCode ?? "",
                        d.Item?.IsVatEnables,
                        d.ItemInstances?.Select(x => x.SerialNumber).ToList() ?? new List<string>()
                    )).ToList() ?? new List<PurchaseItemsDto>()
                );

                return Result<GetPurchaseDetailsQueryResponse>.Success(purchaseResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching purchase details by referenceNo", ex);
            }
        }

        public async Task<Result<PagedResult<FilterPurchaseQuotationQueryResponse>>> GetPurchaseQuotationFilter(PaginationRequest paginationRequest, FilterPurchaseDetailsDTOs filterPurchaseDetailsDTOs)
        {
            try
               {
        var (purchaseQuotationDetails, schoolId, institutionId, userRole, isSuperAdmin) =
            await _getUserScopedData.GetUserScopedData<PurchaseQuotationDetails>();

        var filterItems = isSuperAdmin
            ? purchaseQuotationDetails
            : purchaseQuotationDetails.Where(x =>
                x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

        var schoolIds = await _unitOfWork.BaseRepository<School>()
            .GetConditionalFilterType(
                x => x.InstitutionId == institutionId,
                query => query.Select(c => c.Id)
            );

      
        DateTime? startDateUtc = null;
        DateTime? endDateUtc = null;

        if (!string.IsNullOrWhiteSpace(filterPurchaseDetailsDTOs.startDate?.Trim()))
        {
            startDateUtc = await _dateConverterHelper.ConvertToEnglish(filterPurchaseDetailsDTOs.startDate.Trim());

            if (DateTime.TryParse(filterPurchaseDetailsDTOs.startDate, out var tempStart) &&
                tempStart.TimeOfDay == TimeSpan.Zero)
            {
                endDateUtc = startDateUtc?.AddDays(1);
            }
        }

        if (!string.IsNullOrWhiteSpace(filterPurchaseDetailsDTOs.endDate?.Trim()))
        {
            endDateUtc = await _dateConverterHelper.ConvertToEnglish(filterPurchaseDetailsDTOs.endDate.Trim());

            if (DateTime.TryParse(filterPurchaseDetailsDTOs.endDate, out var tempEnd) &&
                tempEnd.TimeOfDay == TimeSpan.Zero)
            {
                endDateUtc = endDateUtc?.AddDays(1);
            }
        }

        var userId = _tokenService.GetUserId;

   
        var purchaseDetailsResult = await _unitOfWork.BaseRepository<PurchaseQuotationDetails>()
            .GetConditionalAsync(
                predicate: x =>
                    x.CreatedBy == userId() &&
                 
                    (string.IsNullOrEmpty(filterPurchaseDetailsDTOs.ledgerId) ||
                     x.LedgerId.ToLower() == filterPurchaseDetailsDTOs.ledgerId.ToLower()) &&
                  
                    (startDateUtc == null || x.CreatedAt >= startDateUtc) &&
                    (endDateUtc == null || x.CreatedAt <= endDateUtc),
                queryModifier: q => q.Include(sd => sd.PurchaseQuotationItems)
                
            );

        // --- Mapping ---
        var responseList = purchaseDetailsResult.Select(p =>
        {

            decimal subTotalAmount = p.PurchaseQuotationItems?.Sum(i => i.Amount) ?? 0m;

            decimal? taxableAmount = subTotalAmount - p.DiscountAmount;
            return new FilterPurchaseQuotationQueryResponse(
                 p.Id,
                        p.Date,
                        p.QuotationNumber,
                        p.LedgerId,
                        p.AmountInWords,
                        p.DiscountPercent,
                        p.DiscountAmount,
                        p.VatPercent,
                        p.VatAmount,
                        p.SchoolId,
                        p.GrandTotalAmount,
                        p.ReferenceNumber,
                        p.StockCenterId,
                        p.QuotationStatuss,
                                                     taxableAmount,
    subTotalAmount,
                        p.PurchaseQuotationItems?.Select(i => new PurchaseQuotationItemsDto(
                           i.Id,
                           i.Quantity,
                           i.UnitId,
                           i.ItemId,
                           i.Price,
                           i.Amount,
                           i.CreatedBy,
                           i.CreatedAt,
                           i.UpdatedBy,
                           i.UpdatedAt,
                           i.PurchaseQuotationDetailsId,
                           i.IsDeleted
                           )).ToList() ?? new List<PurchaseQuotationItemsDto>());
        }).ToList();

                
                int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
        int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;
        int totalItems = responseList.Count;

        var pagedItems = responseList
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var pagedResult = new PagedResult<FilterPurchaseQuotationQueryResponse>
        {
            Items = pagedItems,
            TotalItems = totalItems,
            PageIndex = pageIndex,
            pageSize = pageSize
        };

        return Result<PagedResult<FilterPurchaseQuotationQueryResponse>>.Success(pagedResult);
    }
    catch (Exception ex)
    {
        throw new Exception($"An error occurred while fetching purchase details: {ex.Message}", ex);
    }
        }

        public async Task<Result<string>> GetCurrentPurchaseBillNumber()
        {
            string schoolId = _tokenService.SchoolId().FirstOrDefault() ?? string.Empty;

            var fiscalYear = await _settingServices.GetCurrentFiscalYearBy(schoolId, default);

            var school = await _unitOfWork.BaseRepository<School>().GetByGuIdAsync(schoolId);
            if (school == null)
            {
                return Result<string>.Failure("Invalid SchoolId. School does not exist.");
            }
            var billNumber = "";
            if (school.BillNumberGenerationTypeForPurchase == BillNumberGenerationType.Manual)
            {
                return Result<string>.Failure("Billnumber for purchase is manual");
            }
            else
            {
                billNumber = await _billNumberGenerator.GenerateBillNumberAsync(schoolId, "purchase", fiscalYear.Data.fyName);
            };

            if(string.IsNullOrEmpty(billNumber))
            {
                return Result<string>.Failure("Failed to generate bill number. Please check company settings.");
            }

            return Result<string>.Success(billNumber);
        }

        public async Task<Result<string>> GetCurrentPurchaseReferenceNumber()
        {
            string schoolId = _tokenService.SchoolId().FirstOrDefault() ?? string.Empty;

            var schoolSettings = await _unitOfWork.BaseRepository<SchoolSettings>().FirstOrDefaultAsync(x=>x.SchoolId == schoolId);
            if (schoolSettings == null)
            {
                return Result<string>.Failure("Invalid School. School Id does not exist.");
            }

            string referenceNumber = "";

           
            if (schoolSettings.PurchaseReferences == SchoolSettings.PurchaseReferencesType.Manual)
            {
                return Result<string>.Failure("Reference number for Purchase is manual.");
            }
            else
            {
               
                referenceNumber = await _billNumberGenerator.GenerateReferenceNumber(schoolId, SchoolSettings.ReferenceType.Purchase);
            }

            if (string.IsNullOrEmpty(referenceNumber))
            {
                return Result<string>.Failure("Failed to generate purchase reference number. Please check company settings.");
            }

            return Result<string>.Success(referenceNumber);
        }

        public async Task<Result<QuotationToPurchaseResponse>> QuotationToPurchase(QuotationToPurchaseCommand quotationToPurchaseCommand)
        {
         
                if (string.IsNullOrEmpty(quotationToPurchaseCommand.purchaseQuotationId))
                {
                    return Result<QuotationToPurchaseResponse>.Failure("InvalidRequest", "Purchase Quotation ID cannot be null or empty");
                }

                var userId = _tokenService.GetUserId();
                var schoolId = _tokenService.SchoolId().FirstOrDefault();
                var FyId = _fiscalContext.CurrentFiscalYearId;
                var newPurchaseId = Guid.NewGuid().ToString();

                var purchaseDetails = await _unitOfWork.BaseRepository<PurchaseQuotationDetails>()
                    .GetConditionalAsync(x => x.Id == quotationToPurchaseCommand.purchaseQuotationId,
                        q => q.Include(rm => rm.PurchaseQuotationItems).ThenInclude(Ii => Ii.ItemInstances));



                var purchaseQuotation = purchaseDetails.FirstOrDefault();


                if (purchaseQuotation == null)
                {
                    return Result<QuotationToPurchaseResponse>.Failure("NotFound", "Purchasae Quotation not found");
                }

                var school = await _unitOfWork.BaseRepository<School>().GetByGuIdAsync(schoolId);
                if (school == null)
                {
                    return Result<QuotationToPurchaseResponse>.Failure("Invalid SchoolId. School does not exist.");
                }


                var mappedPurchaseQuotation = new AddPurchaseDetailsCommand(
                     date: purchaseQuotation.Date,
                     billNumber: quotationToPurchaseCommand.billNumbers,
                     ledgerId: purchaseQuotation.LedgerId,
                     amountInWords: purchaseQuotation.AmountInWords,
                     discountPercent: purchaseQuotation.DiscountPercent,
                     discountAmount: purchaseQuotation.DiscountAmount,
                     vatPercent: purchaseQuotation.VatPercent,
                     vatAmount: purchaseQuotation.VatAmount,
                     grandTotalAmount: purchaseQuotation.GrandTotalAmount,
                     paymentId: quotationToPurchaseCommand.paymentId,
                     referenceNumber: purchaseQuotation.ReferenceNumber,
                     isPurchase: true, // you must decide the value here
                     stockCenterId: purchaseQuotation.StockCenterId,
                        chequeNumber: quotationToPurchaseCommand.chequeNumber,
                        bankName: quotationToPurchaseCommand.bankName,
                        accountName: quotationToPurchaseCommand.accountName,
                        purchaseQuotationNumber : purchaseQuotation.PurchaseQuotationNumber,
                        subTotalAmount: purchaseQuotation.SubTotalAmount,
                        taxableAmount: purchaseQuotation.TaxableAmount,
                        amountAfterVat: purchaseQuotation.AmountAfterVat,
                     PurchaseItems: purchaseQuotation.PurchaseQuotationItems.Select(x =>
                         new AddPurchaseItemsRequest(
                             x.Quantity,
                             x.UnitId,
                             x.ItemId,
                             x.Price,
                             x.Amount,
                             x.ItemInstances.Select(x => x.SerialNumber).ToList() ?? new List<string>()
                         //x.ItemInstances.ToList()
                         )
                     ).ToList()
                 );



                var addPurchaseDetails = await Add(mappedPurchaseQuotation);


                var resultDTOs = new QuotationToPurchaseResponse(
                    addPurchaseDetails.Data.Id,
                    addPurchaseDetails.Data.Date,
                    addPurchaseDetails.Data.BillNumber,
                    addPurchaseDetails.Data.LedgerId,
                    addPurchaseDetails.Data.AmountInWords,
                    addPurchaseDetails.Data.DiscountPercent,
                    addPurchaseDetails.Data.DiscountAmount,
                    addPurchaseDetails.Data.VatPercent,
                    addPurchaseDetails.Data.VatAmount,
                    addPurchaseDetails.Data.CreatedBy,
                    addPurchaseDetails.Data.SchoolId,
                    addPurchaseDetails.Data.CreatedAt,
                    addPurchaseDetails.Data.grandTotalAmount,
                    addPurchaseDetails.Data.paymentId,
                    addPurchaseDetails.Data.stockCenterId,
                    addPurchaseDetails.Data.PurchaseItems.Select(x =>
                        new AddPurchaseItemsRequest(
                            x.quantity,
                            x.unitId,
                            x.itemId,
                            x.price,
                            x.amount,
                            x.serialNumbers?.ToList() ?? new List<string>()
                        )
                    ).ToList()
                );

                return Result<QuotationToPurchaseResponse>.Success(resultDTOs);


           
        }

        public async Task<Result<GetPurchaseQuotationByIdQueryResponse>> GetPurchaseQuotationById(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var purchaseQuotationDetails = await _unitOfWork.BaseRepository<PurchaseQuotationDetails>().
                    GetConditionalAsync(x => x.Id == id && x.IsDeleted == false,
                    query => query.Include(rm => rm.PurchaseQuotationItems));

                var p = purchaseQuotationDetails.FirstOrDefault();

                if (p == null)
                {
                    return null;
                }

                var response = new GetPurchaseQuotationByIdQueryResponse(
                        p.Id,
                        p.Date ?? "",
                        p.QuotationNumber ?? "",
                        p.LedgerId,
                        p.AmountInWords,
                        p.DiscountPercent,
                        p.DiscountAmount,
                        p.VatPercent,
                        p.VatAmount,
                        p.SchoolId,
                        p.GrandTotalAmount,
                        p.ReferenceNumber ?? "",
                        p.StockCenterId ?? "",

                    p.PurchaseQuotationItems?.Select(detail =>
                    {
                        return new PurchaseQuotationItemsDto(
                            detail.Id,
                            detail.Quantity,
                            detail.UnitId,
                            detail.ItemId,
                            detail.Price,
                            detail.Amount,
                            detail.CreatedBy,
                            detail.CreatedAt,
                            detail.UpdatedBy,
                            detail.UpdatedAt,
                            detail.PurchaseQuotationDetailsId,
                            detail.IsDeleted

                        );
                    }).ToList() ?? new List<PurchaseQuotationItemsDto>()
                );


                return Result<GetPurchaseQuotationByIdQueryResponse>.Success(response);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching purchase Quotation by id", ex);

            }
        }
    }  
}
    
