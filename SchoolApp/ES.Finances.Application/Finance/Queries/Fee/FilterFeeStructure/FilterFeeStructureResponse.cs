using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Queries.Fee.FilterFeeStructure
{
    public record FilterFeeStructureResponse
    (
        string id,
            string classId,
            decimal? discountAmount,
            string feeCategoryName,
            decimal totalAmount,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
        );
}
