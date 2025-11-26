using ES.Staff.Application.Staff.Command.AddAcademicTeam;
using ES.Staff.Application.Staff.Command.AssignClassToAcademicTeam;
using ES.Staff.Application.Staff.Command.UnAssignedClassToAcademicTeam;
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
        Task<Result<AssignClassResponse>> AssignClass(AssignClassCommand assignClassCommand);
        Task<Result<UnAssignClassResponse>> UnAssignClass(UnAssignClassCommand unAssignClassCommand);
    }
}
