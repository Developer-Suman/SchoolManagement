

using static TN.Authentication.Domain.Entities.School;

namespace TN.Sales.Application.Sales.Queries.BillNumberGenerationBySchool
{
   public record BIllNumberGenerationBySchoolQueryResponse
   (
        BillNumberGenerationType BillNumberGenerationType,
        string schoolId

   );
}
