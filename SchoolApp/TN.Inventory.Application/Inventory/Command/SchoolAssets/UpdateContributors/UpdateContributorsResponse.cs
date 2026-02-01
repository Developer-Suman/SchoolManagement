using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateContributors
{
    public record UpdateContributorsResponse
    (
            string id,
            string name,
            string? organization,
            string? contactNumber,
            string? email,
            string schoolId,
            bool isActive,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
        );
}
