using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterContributors
{
    public record FilterContributorsDTOs
    (
        string? name,
        string? startDate,
        string? endDate
        );
}
