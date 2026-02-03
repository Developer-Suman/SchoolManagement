using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.DeleteSchoolItems
{
    public record DeleteSchoolItemsCommand
    (
        string id    
        ):IRequest<Result<bool>>;
}
