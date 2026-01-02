using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Staff.Application.Staff.Command.AssignClassToAcademicTeam
{
    public record AssignClassCommand
    (
        string AcademicTeamId,
        List<string> ClassIds
        ) : IRequest<Result<AssignClassResponse>>;
}
