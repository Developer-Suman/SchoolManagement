

using static TN.Authentication.Domain.Entities.School;

namespace TN.Setup.Application.Setup.Queries.SchoolById
{
     public record GetSchoolByIdResponse
     (
         
            string id="",
            string name="",
            string address = "",
            string shortName="",
            string email="",
            string contactNumber = "",
            string contactPerson = "",
            string pan = "",
            string imageUrl = "",
            bool isEnabled=false,
            string institutionId="",
            DateTime createdDate=default,
            string createdBy = "",
            DateTime modifiedDate = default,
            string modifiedBy = "",
            bool isDeleted = false,
            string? fyName="",
             BillNumberGenerationType billNumberGenerationTypeForPurchase= BillNumberGenerationType.Automatic,
            BillNumberGenerationType billNumberGenerationTypeForSales= BillNumberGenerationType.Automatic

     );
    
   
}
