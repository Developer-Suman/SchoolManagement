using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.AddFeeStructure
{
    public record AddFeeStructureResponse
    (
        string id,
            decimal amount,
            string classId,
            string fyId,
            string feeTypeId,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
        );
}
