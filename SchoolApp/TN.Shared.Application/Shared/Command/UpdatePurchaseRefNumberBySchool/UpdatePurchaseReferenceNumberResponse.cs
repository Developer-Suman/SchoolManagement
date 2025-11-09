

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdatePurchaseRefNumberBySchool
{
    public record  UpdatePurchaseReferenceNumberResponse
    (
         PurchaseReferencesType purchaseReference = default,
            string schoolId = ""

    );
}
