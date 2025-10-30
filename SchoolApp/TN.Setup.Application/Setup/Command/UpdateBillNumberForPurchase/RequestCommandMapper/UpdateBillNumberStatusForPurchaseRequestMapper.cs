
namespace TN.Setup.Application.Setup.Command.UpdateBillNumberForPurchase.RequestCommandMapper
{
    public static class UpdateBillNumberStatusForPurchaseRequestMapper
    {
        public static UpdateBillNumberStatusForPurchaseCommand ToCommand(this UpdateBillNumberStatusForPurchaseRequest request, string id)
        { 
            return new UpdateBillNumberStatusForPurchaseCommand
                (
                     id,
                     request.billNumberGenerationTypeForPurchase
                );
        }
    }
}
