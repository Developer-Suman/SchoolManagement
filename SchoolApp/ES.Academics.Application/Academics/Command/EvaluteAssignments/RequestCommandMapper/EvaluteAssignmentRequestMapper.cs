using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.EvaluteAssignments.RequestCommandMapper
{
    public static class EvaluteAssignmentRequestMapper
    {
        public static EvaluteAssignmentCommand ToCommand(this EvaluteAssignmentsRequest request)
        {
            return new EvaluteAssignmentCommand
            (request.assignmentId,
            request.studentId,
            request.marks,
            request.teacherRemark
                );
           
        }
    }
}
