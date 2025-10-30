

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdateSalesReturnType
{
    public record  UpdateSalesReturnTypeResponse
    (

        string schoolId,
        PurchaseSalesReturnNumberType salesReturnNumberType


    );
}
