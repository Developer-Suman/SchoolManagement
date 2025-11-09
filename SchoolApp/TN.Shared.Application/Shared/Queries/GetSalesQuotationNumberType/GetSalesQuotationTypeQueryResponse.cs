

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Queries.GetSalesQuotationNumberType
{
    public record  GetSalesQuotationTypeQueryResponse
   (

         string schoolId,
            PurchaseSalesQuotationNumberType salesQuotationNumberType,
            string? salesQuotationNumber

    );
}
