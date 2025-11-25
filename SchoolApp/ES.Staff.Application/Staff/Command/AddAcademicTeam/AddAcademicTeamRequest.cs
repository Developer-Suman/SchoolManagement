using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.Staff.Command.AddAcademicTeam
{
    public record AddAcademicTeamRequest
   (
        string email,
        string username,
        string password,
        string? firstName,
        string? lastName,
        string? address,
        List<string> rolesId
        );
}
