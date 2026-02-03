using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.DeleteContributors
{
    public record DeleteContributorsCommand
    (
        string id
        ) : IRequest<Result<bool>>;
}
