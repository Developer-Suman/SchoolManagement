using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.FilterSubject
{
    public record FilterSubjectDTOs
    (
        string? name,
        string? startDate,
        string? endDate
        );
}
