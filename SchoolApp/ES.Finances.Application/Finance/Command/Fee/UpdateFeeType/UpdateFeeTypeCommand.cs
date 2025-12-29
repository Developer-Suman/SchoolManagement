using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Finances.Application.Finance.Command.Fee.UpdateFeeType
{
    public record UpdateFeeTypeCommand
    ( 
        string id,
        string name,
            string? description,
            NameOfMonths? nameOfMonths
        ): IRequest<Result<UpdateFeeTypeResponse>>;
}
