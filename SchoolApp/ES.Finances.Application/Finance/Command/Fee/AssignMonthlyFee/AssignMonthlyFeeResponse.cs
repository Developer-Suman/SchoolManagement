using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.AssignMonthlyFee
{
    public record AssignMonthlyFeeResponse
    (
        string classId,
        string feeTypeId
        );
}
