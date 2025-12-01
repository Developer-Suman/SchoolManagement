using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.FilterExamSession
{
    public record FilterExamSessionDTOs
    (
              string? name,
        string? startDate,
        string? endDate
        );
}
