using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.SchoolItemEnum;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItems
{
    public record FilterSchoolItemsQueryResponse
    (
         string id,
            string name,
            string contributorId,
            ItemCondition itemCondition,
            DateTime receivedDate,
            decimal? estimatedValue,
                 decimal? quantity,
            UnitType? unitType,
            string schoolId,
            bool isActive,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,

            DateTime modifiedAt
        );
}
