using ES.Staff.Application.Staff.Command.AddAcademicTeam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Staff.Application.ServiceInterface
{
    public interface IAcademicTeamServices
    {
        Task<Result<AddAcademicTeamResponse>> AddAcademicTeam(AddAcademicTeamCommand addAcademicTeamCommand);
    }
}
