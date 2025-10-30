

using TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationBySchool;

namespace TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationByCompany.RequestCommandMapper
{
    public static class UpdateBillNumberGeneratorByCompanyRequestMapper
    {
        public static UpdateBillNumberGeneratorBySchoolCommand ToCommand(this UpdateBillNumberGeneratorBySchoolRequest request, string schoolId)
        {
            return new UpdateBillNumberGeneratorBySchoolCommand
                (
                request.BillNumberGenerationType,
                schoolId
                );
            
        }
    }
}
