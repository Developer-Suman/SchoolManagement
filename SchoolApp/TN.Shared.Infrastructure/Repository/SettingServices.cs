
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using TN.Authentication.Application.Authentication.Queries.GetExpiredDateItemStatusBySchool;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Command.UpdateCurrentFiscalYear;
using TN.Shared.Application.Shared.Command.UpdateExpenseTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdateIncomeTransactionNumberTypeCommand;
using TN.Shared.Application.Shared.Command.UpdateInventoryMethodBySchool;
using TN.Shared.Application.Shared.Command.UpdateItemStatusBySchool;
using TN.Shared.Application.Shared.Command.UpdateJournalRefBySchool;
using TN.Shared.Application.Shared.Command.UpdatePaymentTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdatePurchaseQuotationNumberType;
using TN.Shared.Application.Shared.Command.UpdatePurchaseRefNumberBySchool;
using TN.Shared.Application.Shared.Command.UpdatePurchaseReturnType;
using TN.Shared.Application.Shared.Command.UpdateReceiptTransactionNumberType;
using TN.Shared.Application.Shared.Command.UpdateSalesQuotationNumberType;
using TN.Shared.Application.Shared.Command.UpdateSalesReferenceNumberByCompany;
using TN.Shared.Application.Shared.Command.UpdateSalesReferenceNumberBySchool;
using TN.Shared.Application.Shared.Command.UpdateSalesReturnType;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInPurchase;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInSales;
using TN.Shared.Application.Shared.Queries.GetAllCurrentFiscalYear;
using TN.Shared.Application.Shared.Queries.GetCurrentFiscalYear;
using TN.Shared.Application.Shared.Queries.GetExpenseTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetIncomeTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetInventoryMethodBySchool;
using TN.Shared.Application.Shared.Queries.GetJournalRefBySchool;
using TN.Shared.Application.Shared.Queries.GetPaymentTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetPurchaseQuotationNumber;
using TN.Shared.Application.Shared.Queries.GetPurchaseReferenceNumber;
using TN.Shared.Application.Shared.Queries.GetPurchaseReturnNumber;
using TN.Shared.Application.Shared.Queries.GetReceiptTransactionNumberType;
using TN.Shared.Application.Shared.Queries.GetSalesQuotationNumberType;
using TN.Shared.Application.Shared.Queries.GetSalesReferenceNumber;
using TN.Shared.Application.Shared.Queries.GetSalesReturnNumber;
using TN.Shared.Application.Shared.Queries.GetSerialNumberForPurchase;
using TN.Shared.Application.Shared.Queries.GetTaxStatusInPurchase;
using TN.Shared.Application.Shared.Queries.GetTaxStatusInSales;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using static TN.Authentication.Domain.Entities.SchoolSettings;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Shared.Infrastructure.Repository
{
    public class SettingServices : ISettingServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBillNumberGenerator _billNumberGenerator;
        private readonly ITokenService _tokenService;
        private readonly FiscalContext _fiscalContext;
     


        public SettingServices(FiscalContext fiscalContext,IUnitOfWork unitOfWork,IMapper mapper, IBillNumberGenerator billNumberGenerator, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _billNumberGenerator = billNumberGenerator;
            _tokenService = tokenService;
            _fiscalContext = fiscalContext;

        }

        public async Task<Result<UpdateFiscalYearResponse>> UpdateFiscalYear(string schoolId, string? currentFiscalYearId, CancellationToken cancellationToken)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    bool IsDemoUser = await _tokenService.isDemoUser();
                    string userId = _tokenService.GetUserId();
                    SchoolSettings? fiscalYear;

                    if (IsDemoUser)
                    {
                        fiscalYear = await _unitOfWork.BaseRepository<SchoolSettings>()
                            .GetSingleAsync(x => x.SchoolId == schoolId
                                              && x.UserId == userId);
                    }
                    else
                    {
                        fiscalYear = await _unitOfWork.BaseRepository<SchoolSettings>()
                            .GetSingleAsync(x => x.SchoolId == schoolId
                                              && x.UserId == null);
                    }

                    fiscalYear.CurrentFiscalYearId = currentFiscalYearId;
                    fiscalYear.SchoolId = schoolId;

                    _unitOfWork.BaseRepository<SchoolSettings>().Update(fiscalYear);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var fiscalYearDisplay = new UpdateFiscalYearResponse(fiscalYear.SchoolId, fiscalYear.CurrentFiscalYearId);
                    return Result<UpdateFiscalYearResponse>.Success(fiscalYearDisplay);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("Failed to update current fiscal Year");
                }
            }
        }
        public async Task<Result<PagedResult<GetAllFiscalYearQueryResponse>>> GetAllFiscalYear(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var fiscalYear = await _unitOfWork.BaseRepository<FiscalYears>().GetAllAsyncWithPagination();
                //var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                //        .GetSingleAsync(fy => fy.Id == _fiscalContext.CurrentFiscalYearId && !fy.IsClosed);

                //            var fiscalYearQueryable = await _unitOfWork.BaseRepository<FiscalYears>()
                //.FindBy(fy => !fy.IsClosed );

                fiscalYear = fiscalYear.OrderBy(fy => fy.StartDate);

                var fiscalYearResult = await fiscalYear
                    .AsNoTracking()
                    .ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var fiscalYearResponse = _mapper.Map<PagedResult<GetAllFiscalYearQueryResponse>>(fiscalYearResult.Data);

                return Result<PagedResult<GetAllFiscalYearQueryResponse>>.Success(fiscalYearResponse);

            }
            catch (Exception ex)

            {
                throw new Exception("An error occurred while fetching all fiscal year", ex);
            }
        }

        public async Task<Result<GetCurrentFiscalYearQueryResponse>> GetCurrentFiscalYearBy(string schoolId, CancellationToken cancellationToken)
        {
            try
            {
                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? fiscalYear;

                if (IsDemoUser)
                {
                    fiscalYear = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    fiscalYear = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }


                var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                    .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);


                var fiscalYearData = new GetCurrentFiscalYearQueryResponse(fiscalYear.CurrentFiscalYearId,fiscalYear.SchoolId, currentFiscalYear.StartDate.ToString(), currentFiscalYear.FyName);
                return Result<GetCurrentFiscalYearQueryResponse>.Success(fiscalYearData);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Current Fiscal Year ", ex);
            }
        }

        public async Task<Result<GetInventoryBySchoolIdQueryResponse>> GetInventoryMethodBySchool(string schoolId, CancellationToken cancellationToken)
        {
            try
            {
                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? schoolSettings;

                if (IsDemoUser)
                {
                    schoolSettings = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    schoolSettings = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }

              
                if (schoolSettings == null)
                {
                    return Result<GetInventoryBySchoolIdQueryResponse>.Failure("School settings not found.");
                }

                var response = new GetInventoryBySchoolIdQueryResponse(schoolSettings.InventoryMethod);

                return Result<GetInventoryBySchoolIdQueryResponse>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch inventory method", ex);
            }
        }

      
        public async Task<Result<GetItemStatusBySchoolResponse>> GetItemStatusBySchool(string schoolId, CancellationToken cancellationToken)
        {
            try
            {
                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? expiredDate;

                if (IsDemoUser)
                {
                    expiredDate = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    expiredDate = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }

                var expiredDateStatus = new GetItemStatusBySchoolResponse(expiredDate.ShowExpiredDateInItem, expiredDate.ShowSerialNumberInItem);
                return Result<GetItemStatusBySchoolResponse>.Success(expiredDateStatus);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch ExpiredDateItemStatus", ex);
            }
        }


        public async Task<Result<GetJournalRefBySchoolQueryResponse>> GetJournalRefBySchool(string schoolId, CancellationToken cancellationToken)
        {
            try
            {
                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? journalRef;

                if (IsDemoUser)
                {
                    journalRef = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    journalRef = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }

                if (journalRef == null)
                {
                    return Result<GetJournalRefBySchoolQueryResponse>.Failure("School settings not found.");
                }

                var journalReferencesNumber = journalRef.JournalReference == JournalReferencesType.Automatic
                ? await _billNumberGenerator.GenerateJournalReference(schoolId)
                : string.Empty;

                var journalRefDisplay = new GetJournalRefBySchoolQueryResponse(journalRef.JournalReference, journalRef.SchoolId, journalReferencesNumber);
                return Result<GetJournalRefBySchoolQueryResponse>.Success(journalRefDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch journal reference number");
            }
        }

        public async Task<Result<GetPurchaseReferenceNumberQueryResponse>> GetPurchaseReferenceNumber(string schoolId, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? purchaseRef;

                if (IsDemoUser)
                {
                    purchaseRef = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    purchaseRef = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }



                if (purchaseRef == null)
                {
                    return Result<GetPurchaseReferenceNumberQueryResponse>.Failure("School settings not found.");
                }
                var purchaseReferencesNumber = purchaseRef.PurchaseReferences == PurchaseReferencesType.Automatic
                         ? await _billNumberGenerator.GenerateReferenceNumber(schoolId, ReferenceType.Purchase)
                         : string.Empty;

                var purchaseRefDisplay = new GetPurchaseReferenceNumberQueryResponse(purchaseRef.PurchaseReferences, purchaseRef.SchoolId, purchaseReferencesNumber);
                return Result<GetPurchaseReferenceNumberQueryResponse>.Success(purchaseRefDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch SalesReference Numbers");
            }
        }

        public async Task<Result<GetSalesReferenceNumberQueryResponse>> GetSalesReferenceNumber(string schoolId, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? salesReference;

                if (IsDemoUser)
                {
                    salesReference = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    salesReference = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }

                var salesSerialNumberStatus = new GetSalesReferenceNumberQueryResponse(schoolId, salesReference.ShowReferenceNumberForSales);
                return Result<GetSalesReferenceNumberQueryResponse>.Success(salesSerialNumberStatus);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch SalesReference Numbers");
            }
        }

        public async Task<Result<GetSerialNumberForPurchaseQueryResponse>> GetSerialNumberForPurchase(string schoolId, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? serialNumber;

                if (IsDemoUser)
                {
                    serialNumber = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    serialNumber = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }


                var serialNumberDisplay = new GetSerialNumberForPurchaseQueryResponse( serialNumber.ShowSerialNumberForPurchase, serialNumber.SchoolId);
                return Result<GetSerialNumberForPurchaseQueryResponse>.Success(serialNumberDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Show Serial Number for purchase");
            }
        }

        public async Task<Result<GetTaxStatusInPurchaseResponse>> GetTaxStatusInPurchase(string schoolId, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? taxStatusInPurchase;

                if (IsDemoUser)
                {
                    taxStatusInPurchase = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    taxStatusInPurchase = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }


                var taxStatusInPurchaseDetails = new GetTaxStatusInPurchaseResponse(taxStatusInPurchase.SchoolId, taxStatusInPurchase.ShowTaxInPurchase);
                return Result<GetTaxStatusInPurchaseResponse>.Success(taxStatusInPurchaseDetails);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Show Serial Number for purchase");
            }
        }

        public async Task<Result<GetTaxStatusInSalesResponse>> GetTaxStatusInSales(string schoolId, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? taxStatusInSales;

                if (IsDemoUser)
                {
                    taxStatusInSales = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    taxStatusInSales = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }

                var taxStatusInSalesDetails = new GetTaxStatusInSalesResponse(taxStatusInSales.SchoolId, taxStatusInSales.ShowTaxInSales);
                return Result<GetTaxStatusInSalesResponse>.Success(taxStatusInSalesDetails);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Show Serial Number for purchase");
            }
        }

        

        public async Task<Result<UpdateInventoryMethodResponse>> UpdateInventoryMethodBySchool(string schoolId, InventoryMethodType inventoryMethod, CancellationToken cancellationToken)
        {
            try
            {
                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? inventory;

                if (IsDemoUser)
                {
                    inventory = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    inventory = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }



                inventory.InventoryMethod = inventoryMethod;
                inventory.SchoolId = schoolId;

                _unitOfWork.BaseRepository<SchoolSettings>().Update(inventory);
                await _unitOfWork.SaveChangesAsync();

                var inventoryDisplay = new UpdateInventoryMethodResponse(inventory.InventoryMethod,inventory.SchoolId);
                return Result<UpdateInventoryMethodResponse>.Success(inventoryDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update Inventory Method");
            }
        }

        public async Task<Result<UpdateItemStatusBySchoolResponse>> UpdateItemStatusBySchool(string schoolId, bool isExpiredDate,bool isSerialNo, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? itemStatus;

                if (IsDemoUser)
                {
                    itemStatus = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    itemStatus = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }
                itemStatus.ShowExpiredDateInItem = isExpiredDate;
                itemStatus.ShowSerialNumberInItem = isSerialNo;

                _unitOfWork.BaseRepository<SchoolSettings>().Update(itemStatus);
                await _unitOfWork.SaveChangesAsync();

                var expiredDateStatus = new UpdateItemStatusBySchoolResponse(itemStatus.SchoolId, itemStatus.ShowExpiredDateInItem, itemStatus.ShowSerialNumberInItem);
                return Result<UpdateItemStatusBySchoolResponse>.Success(expiredDateStatus);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch ExpiredDateItemStatus");
            }
        }

        public async Task<Result<UpdateJournalRefBySchoolResponse>> UpdateJournalRefBySchool(string schoolId, JournalReferencesType journalReferences, CancellationToken cancellationToken)
        {
            try
            {


                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? journalRef;

                if (IsDemoUser)
                {
                    journalRef = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    journalRef = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }

                journalRef.JournalReference = journalReferences;
                journalRef.SchoolId = schoolId;

                _unitOfWork.BaseRepository<SchoolSettings>().Update(journalRef);
                await _unitOfWork.SaveChangesAsync();

                var journalRefDisplay = new UpdateJournalRefBySchoolResponse( journalRef.JournalReference, journalRef.SchoolId);
                return Result<UpdateJournalRefBySchoolResponse>.Success(journalRefDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update journal reference number");
            }
        }
        
        public async Task<Result<UpdatePurchaseReferenceNumberResponse>> UpdatePurchaseReferenceNumberBySchool(string schoolId,  PurchaseReferencesType purchaseReferencesType, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? purchaseRef;

                if (IsDemoUser)
                {
                    purchaseRef = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    purchaseRef = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }


                purchaseRef.PurchaseReferences = purchaseReferencesType;
                purchaseRef.SchoolId = schoolId;

                _unitOfWork.BaseRepository<SchoolSettings>().Update(purchaseRef);
                await _unitOfWork.SaveChangesAsync();

                var salesResult = new UpdatePurchaseReferenceNumberResponse(purchaseRef.PurchaseReferences, purchaseRef.SchoolId);
                return Result<UpdatePurchaseReferenceNumberResponse>.Success(salesResult);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Reference number for purchase");
            }
        }

        public async Task<Result<UpdateSalesReferenceNumberResponse>> UpdateSalesReferenceNumberBySchool(string schoolId, bool showReferenceNumberForSales, CancellationToken cancellationToken)
        {
            try
            {



                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? salesReference;

                if (IsDemoUser)
                {
                    salesReference = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    salesReference = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }


                salesReference.ShowReferenceNumberForSales = showReferenceNumberForSales;
               salesReference.SchoolId= schoolId;

                _unitOfWork.BaseRepository<SchoolSettings>().Update(salesReference);
                await _unitOfWork.SaveChangesAsync();

                var salesResult = new UpdateSalesReferenceNumberResponse(
                    salesReference.ShowReferenceNumberForSales, salesReference.SchoolId);
                return Result<UpdateSalesReferenceNumberResponse>.Success(salesResult);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Reference number for sales");
            }
        }

        public async Task<Result<UpdateTaxStatusInPurchaseResponse>> UpdateTaxStatusInPurchaseBy(string schoolId, bool showTaxInPurchase, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? taxStatus;

                if (IsDemoUser)
                {
                    taxStatus = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    taxStatus = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }


                taxStatus.ShowTaxInPurchase = showTaxInPurchase;
                taxStatus.SchoolId = schoolId;

                _unitOfWork.BaseRepository<SchoolSettings>().Update(taxStatus);
                await _unitOfWork.SaveChangesAsync();

                var purchaseResult = new UpdateTaxStatusInPurchaseResponse(taxStatus.SchoolId,taxStatus.ShowTaxInPurchase);
                return Result<UpdateTaxStatusInPurchaseResponse>.Success(purchaseResult);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch tax Status for purchase");
            }
        }

        public async Task<Result<UpdateTaxStatusInSalesResponse>> UpdateTaxStatusInSalesBy(string schoolId, bool showTaxInSales, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? taxStatus;

                if (IsDemoUser)
                {
                    taxStatus = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    taxStatus = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }

                taxStatus.ShowTaxInSales = showTaxInSales;
                taxStatus.SchoolId = schoolId;

                _unitOfWork.BaseRepository<SchoolSettings>().Update(taxStatus);
                await _unitOfWork.SaveChangesAsync();

                var salesResult = new UpdateTaxStatusInSalesResponse(taxStatus.SchoolId, taxStatus.ShowTaxInPurchase);
                return Result<UpdateTaxStatusInSalesResponse>.Success(salesResult);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch tax Status for sales");
            }
        }

        public async Task<Result<ReceiptTransactionNumberTypeResponse>> GetReceiptTransactionType(string schoolId, CancellationToken cancellationToken)
        {

            bool IsDemoUser = await _tokenService.isDemoUser();
            string userId = _tokenService.GetUserId();
            SchoolSettings? receiptTransactionType;

            if (IsDemoUser)
            {
                receiptTransactionType = await _unitOfWork.BaseRepository<SchoolSettings>()
                    .GetSingleAsync(x => x.SchoolId == schoolId
                                      && x.UserId == userId);
            }
            else
            {
                receiptTransactionType = await _unitOfWork.BaseRepository<SchoolSettings>()
                    .GetSingleAsync(x => x.SchoolId == schoolId
                                      && x.UserId == null);
            }


            if (receiptTransactionType == null)
            {
                return Result<ReceiptTransactionNumberTypeResponse>.Failure("School settings not found.");
            }

            var fiscalYear = await _unitOfWork.BaseRepository<SchoolSettings>().GetSingleAsync(x => x.SchoolId == schoolId);

            var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);

            var receiptTransactionNumberType = receiptTransactionType.ReceiptTransactionNumberType == TransactionNumberType.Automatic
            ? await _billNumberGenerator.GenerateTransactionNumber(schoolId, "RECEIPT", currentFiscalYear.FyName)
            : string.Empty;

            var receiptTransactionTypeDisplay = new ReceiptTransactionNumberTypeResponse(
                receiptTransactionType.ReceiptTransactionNumberType, receiptTransactionType.SchoolId, receiptTransactionNumberType);
            return Result<ReceiptTransactionNumberTypeResponse>.Success(receiptTransactionTypeDisplay);
        }

        public async Task<Result<UpdateReceiptTransactionNumberTypeResponse>> UpdateReceiptTransactionType(string schoolid, TransactionNumberType transactionNumberType, CancellationToken cancellationToken)
        {

            bool IsDemoUser = await _tokenService.isDemoUser();
            string userId = _tokenService.GetUserId();
            SchoolSettings? receiptTransactionType;

            if (IsDemoUser)
            {
                receiptTransactionType = await _unitOfWork.BaseRepository<SchoolSettings>()
                    .GetSingleAsync(x => x.SchoolId == schoolid
                                      && x.UserId == userId);
            }
            else
            {
                receiptTransactionType = await _unitOfWork.BaseRepository<SchoolSettings>()
                    .GetSingleAsync(x => x.SchoolId == schoolid
                                      && x.UserId == null);
            }


            if (receiptTransactionType is null)
                return Result<UpdateReceiptTransactionNumberTypeResponse>.Failure("School settings not found.");

            receiptTransactionType.ReceiptTransactionNumberType = transactionNumberType;

            _unitOfWork.BaseRepository<SchoolSettings>().Update(receiptTransactionType);
            await _unitOfWork.SaveChangesAsync();

            return Result<UpdateReceiptTransactionNumberTypeResponse>.Success(
                new UpdateReceiptTransactionNumberTypeResponse(
                    receiptTransactionType.ReceiptTransactionNumberType, receiptTransactionType.SchoolId)
            );

        }

        public async Task<Result<UpdatePaymentTransactionNumberTypeResponse>> UpdatePaymentTransactionType(string schoolId, TransactionNumberType transactionNumberType, CancellationToken cancellationToken)
        {

            bool IsDemoUser = await _tokenService.isDemoUser();
            string userId = _tokenService.GetUserId();
            SchoolSettings? receiptTransactionType;

            if (IsDemoUser)
            {
                receiptTransactionType = await _unitOfWork.BaseRepository<SchoolSettings>()
                    .GetSingleAsync(x => x.SchoolId == schoolId
                                      && x.UserId == userId);
            }
            else
            {
                receiptTransactionType = await _unitOfWork.BaseRepository<SchoolSettings>()
                    .GetSingleAsync(x => x.SchoolId == schoolId
                                      && x.UserId == null);
            }


            if (receiptTransactionType is null)
                return Result<UpdatePaymentTransactionNumberTypeResponse>.Failure("School settings not found.");

            receiptTransactionType.PaymentTransactionNumberType = transactionNumberType;

            _unitOfWork.BaseRepository<SchoolSettings>().Update(receiptTransactionType);
            await _unitOfWork.SaveChangesAsync();

            return Result<UpdatePaymentTransactionNumberTypeResponse>.Success(
                new UpdatePaymentTransactionNumberTypeResponse(
                    receiptTransactionType.ReceiptTransactionNumberType, receiptTransactionType.SchoolId)
            );
        }

        public async Task<Result<GetPaymentTransactionNumberTypeResponse>> GetPaymentTransactionType(string schoolId, CancellationToken cancellationToken)
        {

            bool IsDemoUser = await _tokenService.isDemoUser();
            string userId = _tokenService.GetUserId();
            SchoolSettings? paymentTransactionType;

            if (IsDemoUser)
            {
                paymentTransactionType = await _unitOfWork.BaseRepository<SchoolSettings>()
                    .GetSingleAsync(x => x.SchoolId == schoolId
                                      && x.UserId == userId);
            }
            else
            {
                paymentTransactionType = await _unitOfWork.BaseRepository<SchoolSettings>()
                    .GetSingleAsync(x => x.SchoolId == schoolId
                                      && x.UserId == null);
            }

            if (paymentTransactionType == null)
            {
                return Result<GetPaymentTransactionNumberTypeResponse>.Failure("School settings not found.");
            }

            var fiscalYear = await _unitOfWork.BaseRepository<SchoolSettings>().GetSingleAsync(x => x.SchoolId == schoolId);

            var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);

            var paymentTransactionNumberType = paymentTransactionType.PaymentTransactionNumberType == TransactionNumberType.Automatic
            ? await _billNumberGenerator.GenerateTransactionNumber(schoolId, "PAYMENT", currentFiscalYear.FyName)
            : string.Empty;

            var paymentTransactionTypeDisplay = new GetPaymentTransactionNumberTypeResponse(
                paymentTransactionType.PaymentTransactionNumberType, paymentTransactionType.SchoolId, paymentTransactionNumberType);
            return Result<GetPaymentTransactionNumberTypeResponse>.Success(paymentTransactionTypeDisplay);
        }

        public async Task<Result<GetIncomeTransactionNumberTypeResponse>> GetIncomeTransactionType(string schoolId, CancellationToken cancellationToken)
        {

            bool IsDemoUser = await _tokenService.isDemoUser();
            string userId = _tokenService.GetUserId();
            SchoolSettings? incomeTransactionType;

            if (IsDemoUser)
            {
                incomeTransactionType = await _unitOfWork.BaseRepository<SchoolSettings>()
                    .GetSingleAsync(x => x.SchoolId == schoolId
                                      && x.UserId == userId);
            }
            else
            {
                incomeTransactionType = await _unitOfWork.BaseRepository<SchoolSettings>()
                    .GetSingleAsync(x => x.SchoolId == schoolId
                                      && x.UserId == null);
            }



            var fiscalYear = await _unitOfWork.BaseRepository<SchoolSettings>().GetSingleAsync(x => x.SchoolId == schoolId);

            var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);


            if (incomeTransactionType == null)
            {
                return Result<GetIncomeTransactionNumberTypeResponse>.Failure("School settings not found.");
            }

            var incomeTransactionNumberType = incomeTransactionType.IncomeTransactionNumberType == TransactionNumberType.Automatic
            ? await _billNumberGenerator.GenerateTransactionNumber(schoolId, "INCOME", currentFiscalYear.FyName)
            : string.Empty;

            var incomeTransactionTypeDisplay = new GetIncomeTransactionNumberTypeResponse(
                incomeTransactionType.IncomeTransactionNumberType, incomeTransactionType.SchoolId, incomeTransactionNumberType);
            return Result<GetIncomeTransactionNumberTypeResponse>.Success(incomeTransactionTypeDisplay);

        }

        public async Task<Result<GetExpenseTransactionNumberTypeResponse>> GetExpenseTransactionType(string schoolId, CancellationToken cancellationToken)
        {
            bool IsDemoUser = await _tokenService.isDemoUser();
            string userId = _tokenService.GetUserId();
            SchoolSettings? expenseTransactionType;

            if (IsDemoUser)
            {
                expenseTransactionType = await _unitOfWork.BaseRepository<SchoolSettings>()
                    .GetSingleAsync(x => x.SchoolId == schoolId
                                      && x.UserId == userId);
            }
            else
            {
                expenseTransactionType = await _unitOfWork.BaseRepository<SchoolSettings>()
                    .GetSingleAsync(x => x.SchoolId == schoolId
                                      && x.UserId == null);
            }


            if (expenseTransactionType == null)
            {
                return Result<GetExpenseTransactionNumberTypeResponse>.Failure("School settings not found.");
            }

            var fiscalYear = await _unitOfWork.BaseRepository<SchoolSettings>().GetSingleAsync(x => x.SchoolId == schoolId);

            var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);

            var expenseTransactionNumberType = expenseTransactionType.ExpensesTransactionNumberType == TransactionNumberType.Automatic
            ? await _billNumberGenerator.GenerateTransactionNumber(schoolId, "EXPENSES", currentFiscalYear.FyName)
            : string.Empty;

            var expenseTransactionTypeDisplay = new GetExpenseTransactionNumberTypeResponse(
                expenseTransactionType.ExpensesTransactionNumberType, expenseTransactionType.SchoolId, expenseTransactionNumberType);
            return Result<GetExpenseTransactionNumberTypeResponse>.Success(expenseTransactionTypeDisplay);
        }

        public async Task<Result<UpdateExpenseTransactionNumberTypeResponse>> UpdateExpenseTransactionType(string schoolId, TransactionNumberType transactionNumberType, CancellationToken cancellationToken)
        {
            bool IsDemoUser = await _tokenService.isDemoUser();
            string userId = _tokenService.GetUserId();
            SchoolSettings? expenseTransactionType;

            if (IsDemoUser)
            {
                expenseTransactionType = await _unitOfWork.BaseRepository<SchoolSettings>()
                    .GetSingleAsync(x => x.SchoolId == schoolId
                                      && x.UserId == userId);
            }
            else
            {
                expenseTransactionType = await _unitOfWork.BaseRepository<SchoolSettings>()
                    .GetSingleAsync(x => x.SchoolId == schoolId
                                      && x.UserId == null);
            }

            if (expenseTransactionType is null)
                return Result<UpdateExpenseTransactionNumberTypeResponse>.Failure("School settings not found.");

            expenseTransactionType.ExpensesTransactionNumberType = transactionNumberType;

            _unitOfWork.BaseRepository<SchoolSettings>().Update(expenseTransactionType);
            await _unitOfWork.SaveChangesAsync();

            return Result<UpdateExpenseTransactionNumberTypeResponse>.Success(
                new UpdateExpenseTransactionNumberTypeResponse(
                    expenseTransactionType.ExpensesTransactionNumberType, expenseTransactionType.SchoolId)
            );
        }

        public async Task<Result<UpdateIncomeTransactionNumberTypeResponse>> UpdateIncomeTransactionType(string schoolId, TransactionNumberType transactionNumberType, CancellationToken cancellationToken)
        {

            bool IsDemoUser = await _tokenService.isDemoUser();
            string userId = _tokenService.GetUserId();
            SchoolSettings? incomeTransactionType;

            if (IsDemoUser)
            {
                incomeTransactionType = await _unitOfWork.BaseRepository<SchoolSettings>()
                    .GetSingleAsync(x => x.SchoolId == schoolId
                                      && x.UserId == userId);
            }
            else
            {
                incomeTransactionType = await _unitOfWork.BaseRepository<SchoolSettings>()
                    .GetSingleAsync(x => x.SchoolId == schoolId
                                      && x.UserId == null);
            }

            if (incomeTransactionType is null)
                return Result<UpdateIncomeTransactionNumberTypeResponse>.Failure("School settings not found.");

            incomeTransactionType.IncomeTransactionNumberType = transactionNumberType;

            _unitOfWork.BaseRepository<SchoolSettings>().Update(incomeTransactionType);
            await _unitOfWork.SaveChangesAsync();

            return Result<UpdateIncomeTransactionNumberTypeResponse>.Success(
                new UpdateIncomeTransactionNumberTypeResponse(
                    incomeTransactionType.IncomeTransactionNumberType, incomeTransactionType.SchoolId)
            );
        }

        public async Task<Result<GetPurchaseReturnNumberQueryResponse>> GetPurchaseReturnNumber(string schoolId, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? schoolSettings;

                if (IsDemoUser)
                {
                    schoolSettings = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    schoolSettings = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }


                if (schoolSettings == null)
                {
                    return Result<GetPurchaseReturnNumberQueryResponse>.Failure("school settings not found.");
                }

                string purchaseReturnNumber = string.Empty;


                var fiscalYear = await _unitOfWork.BaseRepository<SchoolSettings>().GetSingleAsync(x => x.SchoolId == schoolId);

                var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                    .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);

                if (schoolSettings.PurchaseReturnNumberType == SchoolSettings.PurchaseSalesReturnNumberType.Automatic)
                {
                    purchaseReturnNumber = await _billNumberGenerator
                        .GenerateTransactionNumber(schoolId, "PURCHASERETURN", currentFiscalYear.FyName);
                }

                var response = new GetPurchaseReturnNumberQueryResponse(
                    schoolId,
                    schoolSettings.PurchaseReturnNumberType,
                    purchaseReturnNumber

                );

                return Result<GetPurchaseReturnNumberQueryResponse>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Purchase Return Number", ex);
            }
        }

        public async Task<Result<GetSalesReturnNumberQueryResponse>> GetSalesReturnNumber(string schoolId, CancellationToken cancellationToken)
        {
            try
            {


                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? schoolSettings;

                if (IsDemoUser)
                {
                    schoolSettings = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    schoolSettings = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }

                if (schoolSettings == null)
                {
                    return Result<GetSalesReturnNumberQueryResponse>.Failure("School settings not found.");
                }

                string salesReturnNumber = string.Empty;


                var fiscalYear = await _unitOfWork.BaseRepository<SchoolSettings>().GetSingleAsync(x => x.SchoolId == schoolId);

                var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                    .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);


                if (schoolSettings.SalesReturnNumberType == SchoolSettings.PurchaseSalesReturnNumberType.Automatic)
                {
                    salesReturnNumber = await _billNumberGenerator
                        .GenerateTransactionNumber(schoolId, "SALESRETURN", currentFiscalYear.FyName);
                }

                var response = new GetSalesReturnNumberQueryResponse(
                    schoolId,
                    schoolSettings.SalesReturnNumberType,
                    salesReturnNumber
                );

                return Result<GetSalesReturnNumberQueryResponse>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Sales Return Number", ex);
            }
        }

        public async Task<Result<UpdatePurchaseReturnTypeResponse>> UpdatePurchaseReturnType(string schoolId, PurchaseSalesReturnNumberType purchaseReturnNumberType, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? purchaseReturn;

                if (IsDemoUser)
                {
                    purchaseReturn = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    purchaseReturn = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }

                purchaseReturn.PurchaseReturnNumberType = purchaseReturnNumberType;
                purchaseReturn.SchoolId = schoolId;

                _unitOfWork.BaseRepository<SchoolSettings>().Update(purchaseReturn);
                await _unitOfWork.SaveChangesAsync();

                var result = new UpdatePurchaseReturnTypeResponse(purchaseReturn.SchoolId, purchaseReturn.PurchaseReturnNumberType);
                return Result<UpdatePurchaseReturnTypeResponse>.Success(result);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update purchase return Number");
            }
        }

        public async Task<Result<GetPurchaseQuotationNumberQueryResponse>> GetPurchaseQuotationNumber(string schoolId, CancellationToken cancellationToken)
        {
            try
            {
                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? schoolSettins;

                if (IsDemoUser)
                {
                    schoolSettins = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    schoolSettins = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }



                if (schoolSettins == null)
                {
                    return Result<GetPurchaseQuotationNumberQueryResponse>.Failure("School settings not found.");
                }

                string purchaseQuotationNumber = string.Empty;


                var fiscalYear = await _unitOfWork.BaseRepository<SchoolSettings>().GetSingleAsync(x => x.SchoolId == schoolId);

                var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                    .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);

                if (schoolSettins.PurchaseQuotationNumberType == SchoolSettings.PurchaseSalesQuotationNumberType.Automatic)
                {
                    purchaseQuotationNumber = await _billNumberGenerator
                        .GenerateTransactionNumber(schoolId, "PURCHASEQUOTATION", currentFiscalYear.FyName);
                }

                var response = new GetPurchaseQuotationNumberQueryResponse(
                    schoolId,
                    schoolSettins.PurchaseQuotationNumberType,
                    purchaseQuotationNumber
                );

                return Result<GetPurchaseQuotationNumberQueryResponse>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Purchase Quotation Number", ex);
            }
        }

        public async Task<Result<UpdateSalesReturnTypeResponse>> UpdateSalesReturnType(string schoolId, PurchaseSalesReturnNumberType salesReturnNumberType, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? salesReturn;

                if (IsDemoUser)
                {
                    salesReturn = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    salesReturn = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }

                salesReturn.SalesReturnNumberType = salesReturnNumberType;
                salesReturn.SchoolId = schoolId;

                _unitOfWork.BaseRepository<SchoolSettings>().Update(salesReturn);
                await _unitOfWork.SaveChangesAsync();

                var result = new UpdateSalesReturnTypeResponse(salesReturn.SchoolId, salesReturn.SalesReturnNumberType);
                return Result<UpdateSalesReturnTypeResponse>.Success(result);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Update sales return Number");
            }
        }

        public async Task<Result<UpdatePurchaseQuotationTypeResponse>> UpdatePurchaseQuotationType(string schoolId, PurchaseSalesQuotationNumberType purchaseQuotationNumberType, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? quotationNumber;

                if (IsDemoUser)
                {
                    quotationNumber = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    quotationNumber = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }

                quotationNumber.PurchaseQuotationNumberType = purchaseQuotationNumberType;
                quotationNumber.SchoolId = schoolId;

                _unitOfWork.BaseRepository<SchoolSettings>().Update(quotationNumber);
                await _unitOfWork.SaveChangesAsync();

                var result = new UpdatePurchaseQuotationTypeResponse(quotationNumber.SchoolId, quotationNumber.PurchaseQuotationNumberType);
                return Result<UpdatePurchaseQuotationTypeResponse>.Success(result);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update purchase Quotation Number");
            }
        }

        public async Task<Result<GetSalesQuotationTypeQueryResponse>> GetSalesQuotationType(string schoolId, CancellationToken cancellationToken)
        {
            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? schoolSettings;

                if (IsDemoUser)
                {
                    schoolSettings = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    schoolSettings = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }


                if (schoolSettings == null)
                {
                    return Result<GetSalesQuotationTypeQueryResponse>.Failure("CoSchoolmpany settings not found.");
                }

                string salesQuotationNumber = string.Empty;


                var fiscalYear = await _unitOfWork.BaseRepository<SchoolSettings>().GetSingleAsync(x => x.SchoolId == schoolId);

                var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                    .GetSingleAsync(fy => fy.Id == fiscalYear.CurrentFiscalYearId);

                if (schoolSettings.SalesQuotationNumberType == SchoolSettings.PurchaseSalesQuotationNumberType.Automatic)
                {
                    salesQuotationNumber = await _billNumberGenerator
                        .GenerateTransactionNumber(schoolId, "SALESQUOTATION", currentFiscalYear.FyName);
                }

                var response = new GetSalesQuotationTypeQueryResponse(
                    schoolId,
                    schoolSettings.SalesQuotationNumberType,
                    salesQuotationNumber

                );

                return Result<GetSalesQuotationTypeQueryResponse>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch Sales Quotation Number", ex);
            }
        }

        public async Task<Result<UpdateSalesQuotationTypeResponse>> UpdateSalesQuotationType(string schoolId, PurchaseSalesQuotationNumberType salesQuotationNumberType, CancellationToken cancellationToken)
        {

            try
            {

                bool IsDemoUser = await _tokenService.isDemoUser();
                string userId = _tokenService.GetUserId();
                SchoolSettings? quotationNumber;

                if (IsDemoUser)
                {
                    quotationNumber = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == userId);
                }
                else
                {
                    quotationNumber = await _unitOfWork.BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId
                                          && x.UserId == null);
                }

                quotationNumber.SalesQuotationNumberType = salesQuotationNumberType;
                quotationNumber.SchoolId = schoolId;

                _unitOfWork.BaseRepository<SchoolSettings>().Update(quotationNumber);
                await _unitOfWork.SaveChangesAsync();

                var result = new UpdateSalesQuotationTypeResponse(quotationNumber.SchoolId, quotationNumber.SalesQuotationNumberType);
                return Result<UpdateSalesQuotationTypeResponse>.Success(result);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update sales Quotation Number");
            }
        }
    }
}
