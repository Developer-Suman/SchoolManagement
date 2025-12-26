using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Queries.Fee.FeeStructureById
{
    public record FeeStructureByIdResponse
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
