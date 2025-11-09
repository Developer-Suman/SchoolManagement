using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdatePurchaseReturnType.RequestCommandMapper
{
    public static class UpdatePurchaseReturnTypeRequestMapper
    {
        public static UpdatePurchaseReturnTypeCommand ToCommand(this UpdatePurchaseReturnTypeRequest request,string schoolId)
        {
            return new UpdatePurchaseReturnTypeCommand(
              schoolId,
                request.purchaseReturnNumberType
            );
        }
    }
}
