using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Enum.SchoolItemEnum;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItems
{
    public record AddSchoolItemsCommand
    (
        string name,
            string contributorId,
            ItemStatus itemStatus,
            ItemCondition itemCondition,
            DateTime receivedDate,
            decimal? estimatedValue,
              decimal? quantity,
            UnitType? unitType
        ) : IRequest<Result<AddSchoolItemsResponse>>;
}
