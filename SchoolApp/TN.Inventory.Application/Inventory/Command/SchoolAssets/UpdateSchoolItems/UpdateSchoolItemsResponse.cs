using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.SchoolItemEnum;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItems
{
    public record UpdateSchoolItemsResponse
    (
        string id,
            string name,
            string contributorId,
            ItemCondition itemCondition,
            DateTime receivedDate,
            decimal? estimatedValue,
            bool isActive,
              decimal? quantity,
            UnitType? unitType,
            string fiscalYearId
        );
}
