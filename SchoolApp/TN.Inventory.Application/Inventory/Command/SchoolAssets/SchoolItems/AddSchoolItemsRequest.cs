using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.SchoolItemEnum;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItems
{
    public record AddSchoolItemsRequest
    (
            string name,
            string contributorId,
            ItemStatus itemStatus,
            ItemCondition itemCondition,
            DateTime receivedDate,
            decimal? estimatedValue,
            decimal? quantity,
            UnitType? unitType
        );
}
