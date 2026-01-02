using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.Staff.Command.AssignClassToAcademicTeam
{
    public record AssignClassRequest
    (
        string AcademicTeamId,
        List<string> ClassIds
        );
}
