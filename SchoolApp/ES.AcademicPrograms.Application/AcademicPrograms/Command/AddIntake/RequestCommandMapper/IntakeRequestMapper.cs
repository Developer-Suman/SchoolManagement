using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddIntake.RequestCommandMapper
{
    public static class IntakeRequestMapper
    {
        public static AddIntakeCommand ToCommand(this AddIntakeRequest request)
        {
            return new AddIntakeCommand(
                request.month,
                request.deadline,
                request.isOpen,
                request.courseId
            );
        }
    }
}
