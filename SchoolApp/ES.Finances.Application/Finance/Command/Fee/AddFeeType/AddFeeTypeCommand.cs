using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Finances.Application.Finance.Command.Fee.AddFeeType
{
    public record AddFeeTypeCommand
    (
        string name,
            string? description
        ) : IRequest<Result<AddFeeTypeResponse>>;
}
