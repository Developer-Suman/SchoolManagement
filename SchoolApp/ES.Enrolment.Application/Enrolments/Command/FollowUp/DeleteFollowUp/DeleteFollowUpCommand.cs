using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Command.FollowUp.DeleteFollowUp
{
    public record DeleteFollowUpCommand
    (
        string Id
        ) : IRequest<Result<bool>>;
}
