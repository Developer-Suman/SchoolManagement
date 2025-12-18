using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItemHistory.RequestCommandMapper
{
    public static class AddSchoolItemHistoryRequestMapper
    {
        public static AddSchoolItemHistoryCommand ToCommand(this AddSchoolItemHistoryRequest request)
        {
            return new AddSchoolItemHistoryCommand(
                request.schoolItemId,
                request.previousStatus,
                request.currentStatus,
                request.remarks,
                request.actionDate,
                request.actionBy
                );
        }
    }
}
