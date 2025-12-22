using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Queries.Fee.FilterFeeStructure
{
    public record FilterFeeStructureDTOs
    (
        string? classId,
        string? startDate,
        string? endDate
        );
}
