using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Enum;
using ZXing;

namespace ES.Academics.Application.Academics.Command.Events.UpdateEvents
{
    public record UpdateEventsCommand
    (
         string id,
            string title,
            string? descriptions,
            EventType eventsType,
            DateTime eventsDate,
            string participants,
            TimeOnly? eventTime,
            string venue,
            string? chiefGuest,
            string? organizer,
            string? mentor
        ) : IRequest<Result<UpdateEventsResponse>>;
}
