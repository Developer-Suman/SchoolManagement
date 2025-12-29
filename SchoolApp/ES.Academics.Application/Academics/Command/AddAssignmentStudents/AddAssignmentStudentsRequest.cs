using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddAssignmentStudents
{
    public record AddAssignmentStudentsRequest
    (
         string assignmentId,
            string studentId,
            bool isSubmitted,
            DateTime? submittedAt,
            decimal? marks
        );
}
