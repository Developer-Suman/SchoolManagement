using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Enrolment.Application.Enrolments.Command.FollowUp.AddFollowUp
{
    public record AddFollowUpCommand
    (
        string leadId,
            TimeOnly startTime,
            TimeOnly endTime,
            DateTime followUpDate,
            string notes,
            FollowUpStatus followUpStatus
        ) : IRequest<Result<AddFollowUpResponse>>;
}
