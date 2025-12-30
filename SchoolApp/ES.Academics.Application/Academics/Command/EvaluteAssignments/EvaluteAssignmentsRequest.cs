using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.EvaluteAssignments
{
    public record EvaluteAssignmentsRequest
    (
        string assignmentId,
        string studentId,
        decimal marks,
        string? teacherRemark
        );
}
