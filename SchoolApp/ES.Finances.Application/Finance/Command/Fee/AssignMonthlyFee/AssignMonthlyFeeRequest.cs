using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Finances.Application.Finance.Command.Fee.AssignMonthlyFee
{
    public record AssignMonthlyFeeRequest
    (
        string classId,
        NameOfMonths NameOfMonths,
        string feeTypeId
    );
}
