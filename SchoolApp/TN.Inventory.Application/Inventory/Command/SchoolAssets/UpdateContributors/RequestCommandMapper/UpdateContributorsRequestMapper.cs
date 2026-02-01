using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateContributors.RequestCommandMapper
{
    public static class UpdateContributorsRequestMapper
    {
        public static UpdateContributorsCommand ToCommand(this UpdateContributorsRequest request, string id)
        {
            return new UpdateContributorsCommand
                (
                    id,
                    request.name,
                    request.organization,
                    request.contactNumber,
                    request.email
                );
        }
    }
}
