using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.Events.UpdateEvents.RequestCommandMapper
{
    public static class UpdateEventsRequestMapper
    {
        public static UpdateEventsCommand ToCommand(this UpdateEventsRequest request, string Id)
        {
            return new UpdateEventsCommand(
                Id,
                request.title,
                request.descriptions,
                request.eventsType,
                request.eventsDate,
                request.participants,
                request.eventTime,
                request.venue,
                request.chiefGuest,
                request.organizer,
                request.mentor,
                request.schoolId
                );
        }
    }
}
