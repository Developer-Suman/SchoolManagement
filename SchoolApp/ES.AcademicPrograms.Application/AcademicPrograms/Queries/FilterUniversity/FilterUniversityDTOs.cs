using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterUniversity
{
    public record FilterUniversityDTOs
    (
        string? name,
        string? startDate,
        string? endDate
        );
}
