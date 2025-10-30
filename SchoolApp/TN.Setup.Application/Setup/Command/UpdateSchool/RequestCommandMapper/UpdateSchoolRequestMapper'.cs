


namespace TN.Setup.Application.Setup.Command.UpdateSchool.RequestCommandMapper
{
   public static class UpdateCompanyRequestMapper_
    {
        public static UpdateSchoolCommand ToCommand(this UpdateSchoolRequest request, string organizationId)
        { 
            return new UpdateSchoolCommand
                (
                      organizationId,
                      request.name,
                      request.address,
                      request.shortName,
                      request.email,
                      request.contactNumber,
                      request.contactPerson,
                      request.pan,
                      request.imageUrl,
                      request.isEnabled,
                      request.institutionId,
                      request.isDeleted,
                      request.billNumberGenerationTypeForPurchase,
                      request.BillNumberGenerationTypeForSales
                
                );

        }

    }
}
