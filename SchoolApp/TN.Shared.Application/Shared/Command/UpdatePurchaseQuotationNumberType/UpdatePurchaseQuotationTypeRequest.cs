using static TN.Authentication.Domain.Entities.SchoolSettings;


namespace TN.Shared.Application.Shared.Command.UpdatePurchaseQuotationNumberType
{
    public  record UpdatePurchaseQuotationTypeRequest
    (       
        PurchaseSalesQuotationNumberType purchaseQuotationNumberType

    );
}
