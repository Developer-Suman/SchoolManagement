using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Staff.Application.Staff.Command.UnAssignedClassToAcademicTeam
{
    public record UnAssignClassCommand
   (
         string AcademicTeamId,
        string ClassesId
        ) : IRequest<Result<UnAssignClassResponse>>;
}
