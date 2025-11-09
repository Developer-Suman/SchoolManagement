

using static TN.Authentication.Domain.Entities.School;

namespace TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationBySchool
{
    public record UpdateBillNumberGeneratorBySchoolRequest
    (
        BillNumberGenerationType BillNumberGenerationType
        );
}
