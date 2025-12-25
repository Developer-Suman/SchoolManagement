using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.AssignMonthlyFee.RequestCommandMapper
{
    public static class AssignMonthlyFeeRequestMapper
    {
        public static AssignMonthlyFeeCommand ToCommand(this AssignMonthlyFeeRequest request)
        {
            return new AssignMonthlyFeeCommand
                (
                request.classId,
                request.NameOfMonths,
                request.feeTypeId
                );
        }
    }
}
