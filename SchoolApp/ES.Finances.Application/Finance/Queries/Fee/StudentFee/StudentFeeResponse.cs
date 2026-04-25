using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Entities.Finance.StudentFee;

namespace ES.Finances.Application.Finance.Queries.Fee.StudentFee
{
    public record StudentFeeResponse
    (
        string id="",
            string studentId="",
            string feeStructureId="",

            decimal discount=0,
            decimal totalAmount=0,
            decimal paidAmount=0,
            DateTime dueDate=default,
            bool isActive=true,
            string schoolid="",
            PaidStatus isPaidStatus=PaidStatus.Paid,
            string createdBy="",
            DateTime createdAt=default,
            string modifiedBy="",
            DateTime modifiedAt=default
        );
}
