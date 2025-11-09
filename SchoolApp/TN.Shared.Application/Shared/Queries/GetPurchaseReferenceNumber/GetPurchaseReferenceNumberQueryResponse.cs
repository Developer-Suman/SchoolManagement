

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Queries.GetPurchaseReferenceNumber
{
    public record GetPurchaseReferenceNumberQueryResponse
    (
        PurchaseReferencesType purchaseReference = default,
            string schoolId = "",
            string? purchaseRefNo = ""
        );
}
