using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddAssignmentStudents.RequestCommandMapper
{
    public static class AddAssignmentRequestmapper
    {
        public static AddAssignmentStudentsCommand ToCommand(this AddAssignmentStudentsRequest request)
        {
            return new AddAssignmentStudentsCommand(
                request.assignmentId,
                request.studentIds
                );
        }
    }
}
