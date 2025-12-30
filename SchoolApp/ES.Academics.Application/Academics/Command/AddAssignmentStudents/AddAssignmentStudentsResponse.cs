using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddAssignmentStudents
{
    public record AddAssignmentStudentsResponse
    (
            string assignmentId,
            string schoolId

        );
}
