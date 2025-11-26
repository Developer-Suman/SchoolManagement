using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.Staff.Command.UnAssignedClassToAcademicTeam
{
    public record UnAssignClassResponse
     (
        string AcademicTeamId,
        string ClassesId
        );
}
