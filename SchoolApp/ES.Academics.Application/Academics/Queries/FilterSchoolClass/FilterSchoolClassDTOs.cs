using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.FilterSchoolClass
{
    public record FilterSchoolClassDTOs
    (
        string? name,
        string? startDate,
        string? endDate
        );
}
