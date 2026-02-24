using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterCourse
{
    public record FilterCourseDTOs
    (
        string? title,
        string? startDate,
        string? endDate
        );
}
