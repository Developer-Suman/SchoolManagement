using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItems
{
    public record FilterSchoolItemsDTOs
    (
          string? name,
        string? startDate,
        string? endDate
        );
}
