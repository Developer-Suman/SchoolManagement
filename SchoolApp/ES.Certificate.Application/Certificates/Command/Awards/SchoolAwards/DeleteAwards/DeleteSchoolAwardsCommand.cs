using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.DeleteAwards
{
    public record DeleteSchoolAwardsCommand
    (
        string id
        ): IRequest<Result<bool>>;
}
