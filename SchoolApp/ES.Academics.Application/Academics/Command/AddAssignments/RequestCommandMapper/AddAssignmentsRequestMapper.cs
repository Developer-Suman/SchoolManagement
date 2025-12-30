using ES.Academics.Application.Academics.Command.AddAssignments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddAssignment.RequestCommandMapper
{
    public static class AddAssignmentsRequestMapper
    {
        public static AddAssignmentsCommand ToCommand(this AddAssignmentsRequest request)
        {
            return new AddAssignmentsCommand(
                request.title,
                request.description,
                request.dueDate,
                request.academicTeamId,
                request.classId,
                request.subjectId
                );
        }
    }
}
