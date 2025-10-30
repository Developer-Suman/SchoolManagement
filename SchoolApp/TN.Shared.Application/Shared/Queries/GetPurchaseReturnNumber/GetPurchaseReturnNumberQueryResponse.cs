

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Queries.GetPurchaseReturnNumber
{
    public record  GetPurchaseReturnNumberQueryResponse
    (
         string SchoolId,
        PurchaseSalesReturnNumberType purchaseReturnNumberType,
        string purchaseReturnNumbers=""

    );
}
