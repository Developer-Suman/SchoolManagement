using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddAssignmentToClassSection.RequestCommandMapper
{
    public static class AddAssignmentToClassSectionRequestMapper
    {
        public static AddAssignmentToClassSectionCommand ToCommand(this AddAssignmentToClassSectionRequest request)
        {
            return new AddAssignmentToClassSectionCommand
                (
                request.assignmentId,
                request.classSectionId

                );
        }
    }
}
