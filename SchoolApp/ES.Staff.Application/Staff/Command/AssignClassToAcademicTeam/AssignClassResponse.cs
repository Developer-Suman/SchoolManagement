using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.Staff.Command.AssignClassToAcademicTeam
{
    public record AssignClassResponse
    (
        string AcademicTeamId,
        string ClassesId
        );
}
