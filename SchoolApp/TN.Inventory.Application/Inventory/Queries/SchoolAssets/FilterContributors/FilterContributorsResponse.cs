using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.SchoolItemEnum;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterContributors
{
    public record FilterContributorsResponse
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
