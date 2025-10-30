

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdatePurchaseQuotationNumberType
{
    public record  UpdatePurchaseQuotationTypeResponse
    (

        string schoolId,
            PurchaseSalesQuotationNumberType purchaseQuotationNumberType

    );
}
