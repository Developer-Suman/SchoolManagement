

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Queries.GetPurchaseQuotationNumber
{
    public record GetPurchaseQuotationNumberQueryResponse
    (

            string schoolId,
            PurchaseSalesQuotationNumberType purchaseQuotationNumberType,
            string? purchaseQuotationNumber


    );
}
