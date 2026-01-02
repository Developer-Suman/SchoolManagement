using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.Staff.Command.AssignClassToAcademicTeam.RequestCommandMapper
{
    public static class AssignClassRequestMapper
    {
        public static AssignClassCommand ToCommand(this AssignClassRequest request)
        {
            return new AssignClassCommand(
                request.AcademicTeamId,
                request.ClassIds
            );
        }
    }
}
