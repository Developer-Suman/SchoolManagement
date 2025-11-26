using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.Staff.Command.UnAssignedClassToAcademicTeam.RequestCommandMapper
{
    public static class UnAssignClassRequestMapper
    {
        public static UnAssignClassCommand ToCommand(this UnAssignClassRequest request)
        {
            return new UnAssignClassCommand(
                request.AcademicTeamId,
                request.ClassesId
            );
        }
    }
}
