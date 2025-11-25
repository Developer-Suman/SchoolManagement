using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Staff.Application.Staff.Command.AddAcademicTeam
{
    public record AddAcademicTeamCommand
    (
        string email,
        string username,
        string password,
        string? firstName,
        string? lastName,
        string? address,
        List<string> rolesId
        ) : IRequest<Result<AddAcademicTeamResponse>>;
}
