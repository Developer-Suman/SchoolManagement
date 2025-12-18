using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItemsHistory
{
    public record FilterSchoolItemsHistoryDTOs
    (
         string? schoolItemId,
        string? startDate,
        string? endDate
        );
    
}
