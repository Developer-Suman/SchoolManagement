using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Students;

namespace ES.Staff.Application.Staff.Command.UpdateAcademicTeam
{
    public record UpdateAcademicTeamCommand
    (
        string id,
       string email,
        string username,
        string password,
        string fullName,
            IFormFile? teacherImg,
            int provinceId,
            int districtId,
            int wardNumber,
            string? address,
            GenderStatus gender,
            int? vdcid,
            int? municipalityId,
        List<string> rolesId
        ) : IRequest<Result<UpdateAcademicTeamResponse>>;
}
