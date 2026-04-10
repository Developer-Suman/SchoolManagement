using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Command.Fee.FeeCategory.AddFeeCategory
{
    public record AddFeeCategoryCommand
    (
          string name,
            string description
        ) : IRequest<Result<AddFeeCategoryResponse>>;
}
