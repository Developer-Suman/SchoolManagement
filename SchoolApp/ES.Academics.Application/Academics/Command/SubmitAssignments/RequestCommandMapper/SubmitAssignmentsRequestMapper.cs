using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.SubmitAssignments.RequestCommandMapper
{
    public static class SubmitAssignmentsRequestMapper
    {
        public static SubmitAssignmentsCommand ToCommand(this SubmitAssignmentsRequest request)
        {
            return new SubmitAssignmentsCommand(
                request.assignmentId,
                request.submissionText,
                request.submissionFile
            );
        }
    }
}
