

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdatePurchaseReturnType
{
    public record  UpdatePurchaseReturnTypeRequest
    (
        PurchaseSalesReturnNumberType purchaseReturnNumberType

    );
}
