using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolItemsById
{
    public record SchoolItemsByIdQuery
    (
        string id
        ): IRequest<Result<SchoolItemsByIdResponse>>;
}
