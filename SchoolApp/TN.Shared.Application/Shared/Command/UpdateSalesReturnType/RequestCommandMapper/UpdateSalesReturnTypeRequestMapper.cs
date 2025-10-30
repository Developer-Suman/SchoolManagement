

namespace TN.Shared.Application.Shared.Command.UpdateSalesReturnType.RequestCommandMapper
{
    public static class UpdateSalesReturnTypeRequestMapper
    {
        public static UpdateSalesReturnTypeCommand ToCommand(this UpdateSalesReturnTypeRequest request,string schoolId)
        {
            return new UpdateSalesReturnTypeCommand
                (
                     schoolId,
                     request.salesReturnNumberType


                );
        }
    }
}
