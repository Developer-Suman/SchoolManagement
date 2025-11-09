

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdatePurchaseReturnType
{
    public record  UpdatePurchaseReturnTypeResponse
    (     
        string schoolId,
        PurchaseSalesReturnNumberType purchaseReturnNumberType
        
    );
}
