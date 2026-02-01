using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Enum.SchoolItemEnum;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItemHistory
{
    public record UpdateSchoolItemHistoryCommand
    (
        string id,
        string schoolItemId,
            ItemStatus previousStatus,
            ItemStatus currentStatus,
            string? remarks
        ) : IRequest<Result<UpdateSchoolItemHistoryResponse>>;
}
