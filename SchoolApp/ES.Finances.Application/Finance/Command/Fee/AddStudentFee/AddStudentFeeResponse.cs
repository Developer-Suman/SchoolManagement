using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.AddStudentFee
{
    public record AddStudentFeeResponse
    (
        string id,
            string studentId,
            string feeStructureId,

            decimal discount,
            decimal totalAmount,
            decimal paidAmount,
            DateTime dueDate,
            bool isActive,
            string schoolid,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
        );
}
