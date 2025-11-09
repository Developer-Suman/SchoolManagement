using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.FilterExam
{
    public record FilterExamDTOs
    (
        string? name,
        string? startDate,
        string? endDate
        );
}
