using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItems.RequestCommandMapper
{
    public static class UpdateSchoolItemsRequestmapper
    {
        public static UpdateSchoolItemsCommand ToCommand(this UpdateSchoolitemsRequest request, string id)
        {
            return new UpdateSchoolItemsCommand
                (
                    id,
                    request.name,
                    request.contributorId,
                    request.itemCondition,
                    request.receivedDate,
                    request.estimatedValue,
                    request.quantity,
                    request.unitType
                );
        }
    }
}
