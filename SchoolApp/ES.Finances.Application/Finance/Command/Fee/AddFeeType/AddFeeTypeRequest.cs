using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Finances.Application.Finance.Command.Fee.AddFeeType
{
    public record AddFeeTypeRequest
    (
        string name,
            string? description,
            NameOfMonths? nameOfMonths
        );
}
