using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Queries.Fee.FilterStudentFee
{
    public record FilterStudentFeeResponse
    (
            string studentId,
            List<string> feeStructureId,
            decimal totalAmount,
            decimal paidAmount,
            decimal dueAmount
        );
}
