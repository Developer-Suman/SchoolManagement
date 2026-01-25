using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.Events.AddEvents.RequestCommandMapper
{
    public static class AddEventsRequestMapper
    {
        public static AddEventsCommand ToCommand(this AddEventsRequest request)
        {
            return new AddEventsCommand
                (
                request.title,
                request.descriptions,
                request.eventsType,
                request.eventsDate,
                request.participants,
                request.eventTime,
                request.venue,
                request.chiefGuest,
                request.organizer,
                request.mentor
                );
        }
    }
}
