
using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdateSalesQuotationNumberType
{
    public record  UpdateSalesQuotationTypeResponse
    (

         string schoolId,
            PurchaseSalesQuotationNumberType salesQuotationNumberType


    );
}
