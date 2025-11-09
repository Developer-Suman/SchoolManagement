
using TN.Authentication.Application.Authentication.Queries.GetExpiredDateItemStatusBySchool;
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
using TN.Shared.Domain.ExtensionMethod.Pagination;
using static TN.Authentication.Domain.Entities.SchoolSettings;


namespace TN.Shared.Application.ServiceInterface
{
    public interface ISettingServices
    {
        Task<Result<PagedResult<GetAllFiscalYearQueryResponse>>> GetAllFiscalYear(PaginationRequest paginationRequest,CancellationToken cancellationToken=default);
       
        Task<Result<UpdateFiscalYearResponse>> UpdateFiscalYear(string schoolId,string? currentFiscalYearId, CancellationToken cancellationToken);
        Task<Result<GetTaxStatusInPurchaseResponse>> GetTaxStatusInPurchase(string schoolId, CancellationToken cancellationToken);
        Task<Result<GetTaxStatusInSalesResponse>> GetTaxStatusInSales(string schoolId, CancellationToken cancellationToken);
        Task<Result<GetPurchaseReferenceNumberQueryResponse>> GetPurchaseReferenceNumber(string schoolId, CancellationToken cancellationToken);
        Task<Result<GetSalesReferenceNumberQueryResponse>> GetSalesReferenceNumber(string schoolId, CancellationToken cancellationToken);
        Task<Result<GetItemStatusBySchoolResponse>> GetItemStatusBySchool(string schoolId, CancellationToken cancellationToken);
          Task<Result<UpdateItemStatusBySchoolResponse>> UpdateItemStatusBySchool(string schoolId, bool isExpiredDate, bool isSerialNo, CancellationToken cancellationToken);
          Task<Result<GetJournalRefBySchoolQueryResponse>> GetJournalRefBySchool(string schoolId, CancellationToken cancellationToken);
       
          Task<Result<UpdateJournalRefBySchoolResponse>> UpdateJournalRefBySchool(string schoolId, JournalReferencesType journalReferences, CancellationToken cancellationToken);
          Task<Result<GetInventoryBySchoolIdQueryResponse>> GetInventoryMethodBySchool(string schoolId, CancellationToken cancellationToken);
    
        Task<Result<GetSerialNumberForPurchaseQueryResponse>> GetSerialNumberForPurchase(string schoolId,  CancellationToken cancellationToken);
         Task<Result<UpdateInventoryMethodResponse>> UpdateInventoryMethodBySchool(string schoolId, InventoryMethodType inventoryMethod, CancellationToken cancellationToken);
           Task<Result<UpdateSalesReferenceNumberResponse>> UpdateSalesReferenceNumberBySchool(string schoolId, bool showReferenceNumberForSales, CancellationToken cancellationToken); 
        Task<Result<UpdatePurchaseReferenceNumberResponse>> UpdatePurchaseReferenceNumberBySchool(string schoolId, PurchaseReferencesType purchaseReferencesType, CancellationToken cancellationToken);
   
    Task<Result<UpdateTaxStatusInPurchaseResponse>> UpdateTaxStatusInPurchaseBy(string schoolId, bool showTaxInPurchase, CancellationToken cancellationToken);

        Task<Result<UpdateTaxStatusInSalesResponse>> UpdateTaxStatusInSalesBy(string schoolId, bool showTaxInSales, CancellationToken cancellationToken);

        Task<Result<GetCurrentFiscalYearQueryResponse>> GetCurrentFiscalYearBy(string schoolId,  CancellationToken cancellationToken);

        Task<Result<ReceiptTransactionNumberTypeResponse>> GetReceiptTransactionType(string schoolId, CancellationToken cancellationToken);
        Task<Result<GetPaymentTransactionNumberTypeResponse>> GetPaymentTransactionType(string schoolId, CancellationToken cancellationToken);
        Task<Result<UpdateReceiptTransactionNumberTypeResponse>> UpdateReceiptTransactionType(string schoolId, TransactionNumberType  transactionNumberType, CancellationToken cancellationToken);
        Task<Result<UpdatePaymentTransactionNumberTypeResponse>> UpdatePaymentTransactionType(string schoolId, TransactionNumberType transactionNumberType, CancellationToken cancellationToken);

        Task<Result<GetIncomeTransactionNumberTypeResponse>> GetIncomeTransactionType(string schoolId, CancellationToken cancellationToken);
        Task<Result<GetExpenseTransactionNumberTypeResponse>> GetExpenseTransactionType(string schoolId, CancellationToken cancellationToken);

        Task<Result<UpdateExpenseTransactionNumberTypeResponse>> UpdateExpenseTransactionType(string schoolId, TransactionNumberType transactionNumberType, CancellationToken cancellationToken);

        Task<Result<UpdateIncomeTransactionNumberTypeResponse>> UpdateIncomeTransactionType(string schoolId, TransactionNumberType transactionNumberType, CancellationToken cancellationToken);
   
        Task<Result<GetPurchaseReturnNumberQueryResponse>> GetPurchaseReturnNumber(string schoolId, CancellationToken cancellationToken);
       
        Task<Result<GetSalesReturnNumberQueryResponse>> GetSalesReturnNumber(string schoolId, CancellationToken cancellationToken);
        Task<Result<UpdatePurchaseReturnTypeResponse>> UpdatePurchaseReturnType(string schoolId, PurchaseSalesReturnNumberType purchaseReturnNumberType, CancellationToken cancellationToken);

        Task<Result<UpdateSalesReturnTypeResponse>> UpdateSalesReturnType(string schoolId, PurchaseSalesReturnNumberType salesReturnNumberType, CancellationToken cancellationToken);
        Task<Result<GetPurchaseQuotationNumberQueryResponse>> GetPurchaseQuotationNumber(string schoolId, CancellationToken cancellationToken);

        Task<Result<GetSalesQuotationTypeQueryResponse>> GetSalesQuotationType(string schoolId, CancellationToken cancellationToken);
        Task<Result<UpdatePurchaseQuotationTypeResponse>> UpdatePurchaseQuotationType(string schoolId, PurchaseSalesQuotationNumberType purchaseQuotationNumberType, CancellationToken cancellationToken);
        Task<Result<UpdateSalesQuotationTypeResponse>> UpdateSalesQuotationType(string schoolId, PurchaseSalesQuotationNumberType salesQuotationNumberType, CancellationToken cancellationToken);
    }
}
