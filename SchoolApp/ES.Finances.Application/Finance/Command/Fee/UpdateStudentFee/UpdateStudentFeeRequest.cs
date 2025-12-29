using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.UpdateStudentFee
{
    public record UpdateStudentFeeRequest
    (
        string studentId,
            string feeStructureId,

            decimal discount,
            decimal totalAmount,
            decimal paidAmount
        );
}
