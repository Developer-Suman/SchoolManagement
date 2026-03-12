using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.CourseByUniversity
{
    public record CourseByUniversityResponse
    (
        string id = "",
        string title = ""
        );
}
