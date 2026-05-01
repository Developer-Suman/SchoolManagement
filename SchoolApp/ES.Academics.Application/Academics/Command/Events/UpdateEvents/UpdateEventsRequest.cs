using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;

namespace ES.Academics.Application.Academics.Command.Events.UpdateEvents
{
    public record UpdateEventsRequest
    (
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

        );
}
