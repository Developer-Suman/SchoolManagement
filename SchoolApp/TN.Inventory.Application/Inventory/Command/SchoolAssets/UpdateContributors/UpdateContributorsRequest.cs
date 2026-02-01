using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateContributors
{
    public record UpdateContributorsRequest
    (
         string name,
            string? organization,
            string? contactNumber,
            string? email
        );
}
