using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.Events.AddEvents
{
    public record AddEventsRequest
    (
            string title,
            string? descriptions,
            string eventsType,
            string eventsDate,
            string participants,
            string? eventTime,
            string venue,
            string? chiefGuest,
            string? organizer,
            string? mentor
        );

}
