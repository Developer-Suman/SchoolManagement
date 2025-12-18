using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.Contributors.RequestCommandMapper
{
    public static class AddContributorsRequestMapper
    {
        public static AddContributorsCommand ToCommand(this AddContributorsRequest addContributorsRequest)
        {
            return new AddContributorsCommand
            (
                addContributorsRequest.name,
                addContributorsRequest.organization,
                addContributorsRequest.contactNumber,
                                addContributorsRequest.email
            );
        }
    }
}
