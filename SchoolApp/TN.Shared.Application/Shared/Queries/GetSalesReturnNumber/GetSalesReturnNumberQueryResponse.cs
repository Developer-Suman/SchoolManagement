

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Queries.GetSalesReturnNumber
{
    public record  GetSalesReturnNumberQueryResponse
    (
        string schoolId,
        PurchaseSalesReturnNumberType salesReturnNumberType,
        string? SalesReturnNumber

    );
}
