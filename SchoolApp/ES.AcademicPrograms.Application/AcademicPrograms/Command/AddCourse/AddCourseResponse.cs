using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCourse
{
    public record AddCourseResponse
    (
        string title,
        StudyLevel studyLevel,
        decimal tuationFee,
        string currency,
        string universityId
        );
}
