using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCourse.RequestCommandMapper
{
    public static class AddCourseRequestMapper
    {
        public static AddCourseCommand ToCommand(this AddCourseRequest request)
        {
            return new AddCourseCommand(
                request.title,
                request.studyLevel,
                request.tuationFee,
                request.currency,
                request.universityId
            );
        }
    }
}
