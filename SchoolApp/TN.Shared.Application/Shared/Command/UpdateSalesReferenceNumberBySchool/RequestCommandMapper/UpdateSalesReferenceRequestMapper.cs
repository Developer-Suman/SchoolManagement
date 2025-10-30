
namespace TN.Shared.Application.Shared.Command.UpdateSalesReferenceNumberBySchool.RequestCommandMapper
{
    public static class UpdateSalesReferenceRequestMapper
    {
        public static UpdateSalesReferenceNumberCommand ToCommand(this  UpdateSalesReferenceNumberRequest request,string schoolId) 
        {
            return new UpdateSalesReferenceNumberCommand
                (
                    
                    request.showReferenceNumberForSales,
                    schoolId

                );
        
        }

    }
}
