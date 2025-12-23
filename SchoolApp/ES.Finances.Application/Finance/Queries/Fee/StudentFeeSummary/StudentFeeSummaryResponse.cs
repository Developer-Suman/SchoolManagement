using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Entities.Finance.StudentFee;

namespace ES.Finances.Application.Finance.Queries.Fee.StudentFeeSummary
{
    public record StudentFeeSummaryResponse
    (
        string feeType,
        decimal totalAmount,
        decimal paidAmount,
        decimal dueAmount,
         PaidStatus PaidStatus
        );
}
