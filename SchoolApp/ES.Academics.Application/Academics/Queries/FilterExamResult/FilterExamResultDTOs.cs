using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.FilterExamResult
{
    public record FilterExamResultDTOs
    (
        string? studentId,
        string? startDate,
        string? endDate
        );
}
