using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.SchoolItemEnum;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolItems
{
    public record SchoolItemsResponse
    (
        string id,
        string name,
        ItemStatus itemStatus
        );
}
