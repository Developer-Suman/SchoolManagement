

using static TN.Authentication.Domain.Entities.School;

namespace TN.Purchase.Application.Purchase.Queries.BillNumberGenerationBySchool
{
    public record BillNumberGenerationBySchoolQueryResponse
    (
        BillNumberGenerationType BillNumberGenerationType,
        string schoolId
        );
}
