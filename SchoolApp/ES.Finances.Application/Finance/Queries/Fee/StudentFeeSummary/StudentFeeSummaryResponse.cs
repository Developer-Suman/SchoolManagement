using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Finance;
using static TN.Shared.Domain.Entities.Finance.StudentFee;

namespace ES.Finances.Application.Finance.Queries.Fee.StudentFeeSummary
{
    public record StudentFeeSummaryResponse
    (
        string classId="",
        decimal paidAmount=0,
        PaymentMethods paymentMethod=default,
        decimal totalAmount=0,
         decimal dueAmount = 0
        );  
}
