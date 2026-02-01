using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItemHistory.RequestCommandMapper
{
    public static class UpdateSchoolItemHistoryRequestMapper
    {
        public static UpdateSchoolItemHistoryCommand ToCommand(this UpdateSchoolItemHistoryRequest request, string id)
        {
            return new UpdateSchoolItemHistoryCommand
                (
                    id,
                    request.schoolItemId,
                    request.previousStatus,
                    request.currentStatus,
                    request.remarks
                );
        }
    }
}
