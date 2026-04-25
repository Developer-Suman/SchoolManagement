using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Queries.Fee.FeeCategory.FeeCategoryById
{
    public record FeeCategoryByIdQuery
    (
         string id
        ) : IRequest<Result<FeeCategoryByIdResponse>>;
}
