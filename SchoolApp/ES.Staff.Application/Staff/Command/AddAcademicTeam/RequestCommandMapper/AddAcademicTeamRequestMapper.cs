using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.Staff.Command.AddAcademicTeam.RequestCommandMapper
{
    public static class AddAcademicTeamRequestMapper
    {
        public static AddAcademicTeamCommand ToCommand(this AddAcademicTeamRequest addAcademicTeamRequest)
        {
            return new AddAcademicTeamCommand(
                addAcademicTeamRequest.email,
                addAcademicTeamRequest.username,
                addAcademicTeamRequest.password,
                addAcademicTeamRequest.firstName,
                addAcademicTeamRequest.lastName,
                addAcademicTeamRequest.address,
                addAcademicTeamRequest.rolesId
                );
        }
    }
}
