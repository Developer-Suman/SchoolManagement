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
        string id,
            string studentId,
            string feeStructureId,

            decimal discount,
            decimal totalAmount,
            decimal paidAmount,
            DateTime dueDate,
            bool isActive,
            string schoolid,
            PaidStatus isPaidStatus,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
        );
}
