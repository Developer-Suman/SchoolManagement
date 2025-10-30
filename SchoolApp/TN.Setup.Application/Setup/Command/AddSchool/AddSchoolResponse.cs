

using static TN.Authentication.Domain.Entities.School;

namespace TN.Setup.Application.Setup.Command.AddSchool
{
    public record AddSchoolResponse
    (
          
            string name="",
            string address = "" ,
            string shortName = "",
            string email="",
            string contactNumber = "",
            string contactPerson = "",
            string pan="",
            string imageUrl = "",
            bool isEnabled = true,
            string institutionId = "",
            DateTime createdDate =default,
            string createdBy = "",
            DateTime modifiedDate = default,
            string modifiedBy = "",
            bool isDeleted = true,
            string fiscalYearId="",
            BillNumberGenerationType billNumberGenerationTypeForPurchase= default,
            BillNumberGenerationType BillNumberGenerationTypeForSales = default

    );
}
