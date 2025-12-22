using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Queries.Fee.FilterFeetype
{
    public record FilterFeeTypeDTOs
    (
        string? name,
        string? startDate,
        string? endDate
        );
}
