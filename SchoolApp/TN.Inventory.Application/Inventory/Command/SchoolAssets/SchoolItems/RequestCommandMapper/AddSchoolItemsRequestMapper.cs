using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItems.RequestCommandMapper
{
    public static class AddSchoolItemsRequestMapper
    {
        public static AddSchoolItemsCommand ToCommand(this AddSchoolItemsRequest addSchoolItemsRequest)
        {
            return new AddSchoolItemsCommand
            (
                addSchoolItemsRequest.name,
                addSchoolItemsRequest.contributorId,
                addSchoolItemsRequest.itemStatus,
                addSchoolItemsRequest.itemCondition,
                addSchoolItemsRequest.receivedDate,
                addSchoolItemsRequest.estimatedValue,
                addSchoolItemsRequest.quantity,
                addSchoolItemsRequest.unitType
            );
        }
    }
}
