using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.Events.FilterEvents
{
    public record FilterEventsResponse
    (
        string id,
            string title,
            string? descriptions,
            string eventsType,
            string eventsDate,
            string participants,
            string eventTime,
            string venue,
            string? chiefGuest,
            string? organizer,
            string? mentor,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            bool isActive
        );
}
