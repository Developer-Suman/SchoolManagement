using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.Shared.Command.UpdateInventoryMethodBySchool;

namespace TN.Shared.Application.Shared.Command.UpdateInventoryMethodBySchool.RequestCommandMapper
{
    public static class UpdateInventoryMethodRequestMapper
    {
        public static UpdateInventoryMethodCommand  ToCommand(this UpdateInventoryMethodRequest request, string schoolId)
        {
            return new UpdateInventoryMethodCommand
                (
                    
                    request.inventoryMethod,
                    schoolId
                );
        }
    }
}
