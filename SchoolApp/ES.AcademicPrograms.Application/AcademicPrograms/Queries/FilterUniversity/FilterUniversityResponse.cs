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
            string countryId ="",
            string countryName="",
            string universityAddress="",
            string? descriptions="",
            string? website = "",
            int globalRanking=0,
            List<string> courseNames=default,
               bool isActive= true,
            string schoolId="",
            string createdBy = "",
            DateTime createdAt= default,
            string modifiedBy = "",
            DateTime modifiedAt= default
        );
}
