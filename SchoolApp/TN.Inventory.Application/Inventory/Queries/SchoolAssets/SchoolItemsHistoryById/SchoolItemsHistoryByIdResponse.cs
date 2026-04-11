using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.SchoolItemEnum;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolItemsHistoryById
{
    public record SchoolItemsHistoryByIdResponse
    (
        string id="",
            string schoolItemId="",
            ItemStatus previousStatus=default,
            ItemStatus currentStatus=default,
            string? remarks="",
            string schoolId = "",
            bool isActive=true,
            string createdBy = "",
            DateTime createdAt=default,
            string modifiedBy = "",

            DateTime modifiedAt = default
        );
}
