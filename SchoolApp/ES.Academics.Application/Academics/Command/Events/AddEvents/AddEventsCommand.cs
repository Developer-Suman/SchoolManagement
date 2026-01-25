using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.Events.AddEvents
{
    public record AddEventsCommand
    (
        string title,
            string? descriptions,
            string eventsType,
            DateTime eventsDate,
            string participants,
            DateTime eventTime,
            string venue,
            string? chiefGuest,
            string? organizer,
            string? mentor
        ): IRequest<Result<AddEventsResponse>>;
}
