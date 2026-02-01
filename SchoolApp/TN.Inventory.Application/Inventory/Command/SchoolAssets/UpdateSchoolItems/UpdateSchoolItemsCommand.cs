using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Enum.SchoolItemEnum;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItems
{
    public record UpdateSchoolItemsCommand
    (
        string id,
        string name,
            string contributorId,
            ItemCondition itemCondition,
            DateTime receivedDate,
            decimal? estimatedValue,
            decimal? quantity,
            UnitType? unitType
        ): IRequest<Result<UpdateSchoolItemsResponse>>;
}
