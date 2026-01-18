using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.AddStudentFee
{
    public record AddStudentFeeResponse
    (
        string id="",
            string studentId="",
            string feeStructureId="",

            decimal discount=0,
            decimal totalAmount=0,
            decimal paidAmount = 0,
            bool isActive=true,
            string schoolid = "",
            string createdBy = "",
            DateTime createdAt=default,
            string modifiedBy = "",
            DateTime modifiedAt=default
        );
}
