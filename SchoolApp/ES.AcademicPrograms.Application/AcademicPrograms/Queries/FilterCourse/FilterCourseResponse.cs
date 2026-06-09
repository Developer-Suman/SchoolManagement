using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterCourse
{
    public record FilterCourseResponse
    (
        string id="",
            string title="",
            StudyLevel studyLevel=StudyLevel.Undergraduate,
            decimal tuationFee=0,
            string currency="",
            string universityId="",
            string universityName="",
            bool isActive=true,
            string schoolId = "",
            string createdBy = "",
            DateTime createdAt= default,
            string modifiedBy = "",
            DateTime modifiedAt= default
        );
}
