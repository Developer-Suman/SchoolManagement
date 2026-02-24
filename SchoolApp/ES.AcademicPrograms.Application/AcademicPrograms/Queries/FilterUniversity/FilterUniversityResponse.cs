using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterUniversity
{
    public record FilterUniversityResponse
    (
        string id="",
            string name="",
            string country="",
            string? descriptions="",
            string? website = "",
            int globalRanking=0,
               bool isActive= true,
            string schoolId="",
            string createdBy = "",
            DateTime createdAt= default,
            string modifiedBy = "",
            DateTime modifiedAt= default
        );
}
