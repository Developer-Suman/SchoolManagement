

using static TN.Authentication.Domain.Entities.School;

namespace TN.Setup.Application.Setup.Queries.SchoolByInstitutionId
{
    public record GetSchoolByInstitutionIdResponse
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
            DateTime createdDate,
            string createdBy,
            DateTime modifiedDate,
            string modifiedBy,
            bool isDeleted,
             BillNumberGenerationType billNumberGenerationTypeForPurchase,
            BillNumberGenerationType billNumberGenerationTypeForSales
        );
    
}
