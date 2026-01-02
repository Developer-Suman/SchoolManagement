using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.Staff.Command.UpdateAcademicTeam.RequestCommandmapper
{
    public static class UpdateAcademicTeamRequestMapper
    {
        public static UpdateAcademicTeamCommand ToCommand(this UpdateAcademicTeamRequest request, string id)
        {
            return new UpdateAcademicTeamCommand(
                id,
                request.email,
                request.username,
                request.password,
                request.fullName,
                request.teacherImg,
                request.provinceId,
                request.districtId,
                request.wardNumber,
                request.address,
                request.gender,
                request.vdcid,
                request.municipalityId,
                request.rolesId
            );
        }
    }
}
