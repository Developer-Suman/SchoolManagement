

using static TN.Authentication.Domain.Entities.School;

namespace TN.Setup.Application.Setup.Queries.School
{
   public record GetAllSchoolQueryResponse
    (
            string id="",
            string name="",
            string address="",
            string shortName="",
            string email ="",
            string contactNumber ="",
            string contactPerson ="",
            string pan="",
            string imageUrl="",
            bool isEnabled=default,
            string institutionId="",
            DateTime createdDate=default,
            string createdBy="",
            DateTime modifiedDate=default,
            string modifiedBy="",
            bool isDeleted=default,
             BillNumberGenerationType billNumberGenerationTypeForPurchase=default,
            BillNumberGenerationType billNumberGenerationTypeForSales=default,
            List<SchoolUserDto> Users = default

   );
    public record SchoolUserDto
        (
        string userId=""
        
   );
}
