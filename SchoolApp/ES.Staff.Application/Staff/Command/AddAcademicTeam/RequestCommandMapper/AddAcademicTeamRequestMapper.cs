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
                addAcademicTeamRequest.fullName,
                addAcademicTeamRequest.teacherImg,
                addAcademicTeamRequest.provinceId,
                addAcademicTeamRequest.districtId,
                addAcademicTeamRequest.wardNumber,
                addAcademicTeamRequest.address,
                addAcademicTeamRequest.gender,
                addAcademicTeamRequest.vdcid,
                addAcademicTeamRequest.municipalityId,
                addAcademicTeamRequest.rolesId
                );
        }
    }
}
