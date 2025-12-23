using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Queries.Fee.StudentFeeSummary
{
    public record StudentFeeSummaryDTOs
    (
        string? studentId,
        string? startDate,
        string? endDate
        );
}
