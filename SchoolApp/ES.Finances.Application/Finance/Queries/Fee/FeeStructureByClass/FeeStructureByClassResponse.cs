using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Queries.Fee.FeeStructureByClass
{
    public record FeeStructureByClassResponse
    (
            string id,
            decimal amount,
            string classId,
            string fyId,
            string feeTypeId
        );
}
