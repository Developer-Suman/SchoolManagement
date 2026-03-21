using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;

namespace ES.Academics.Application.Academics.Command.Events.AddEvents
{
    public record AddEventsRequest
    (
            string title,
            string? descriptions,
            EventType eventsType,
            string eventsDate,
            string participants,
            TimeOnly? eventTime,
            string venue,
            string? chiefGuest,
            string? organizer,
            string? mentor
        );

}
