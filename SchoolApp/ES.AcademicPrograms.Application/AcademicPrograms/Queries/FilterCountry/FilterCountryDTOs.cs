using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterCountry
{
    public record FilterCountryDTOs
    (
           string? name,
        string? startDate,
        string? endDate
        );
}
