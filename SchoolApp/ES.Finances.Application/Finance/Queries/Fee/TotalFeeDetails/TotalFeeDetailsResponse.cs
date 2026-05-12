using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Queries.Fee.TotalFeeDetails
{
    public record TotalFeeDetailsResponse
    (
        decimal totalFeeCollected=0,
        decimal totalDueAmount=0,
        decimal totalFeeAmount=0

        );
}
