using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.SchoolItemEnum;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItemHistory
{
    public record UpdateSchoolItemHistoryResponse
    (
                string id,
            string schoolItemId,
            ItemStatus previousStatus,
            ItemStatus currentStatus,
            string? remarks,
            string schoolId,
            bool isActive,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,

            DateTime modifiedAt
        );
}
