

using static TN.Authentication.Domain.Entities.School;

namespace TN.Setup.Application.Setup.Command.UpdateSchool
{
    public record UpdateSchoolRequest
    (
            string id,
            string name,
            string address,
            string shortName,
            string email,
            string contactNumber,
            string contactPerson,
            string pan,
            string imageUrl,
            bool isEnabled,
            string institutionId,
            bool isDeleted,
             BillNumberGenerationType billNumberGenerationTypeForPurchase,
            BillNumberGenerationType BillNumberGenerationTypeForSales

     );
}
