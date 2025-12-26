using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Queries.Fee.FeeStructureById
{
    public record FeeStructureByIdQuery
    (
        string id
        ): IRequest<Result<FeeStructureByIdResponse>>;
}
