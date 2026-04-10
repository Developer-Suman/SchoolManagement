using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Queries.Fee.FeeCategory.FilterFeeCategory
{
    public record FilterFeeCategoryDTOs
    (
        string? name,
        string? startDate,
        string? endDate
        );
}
