using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;

namespace ES.Academics.Application.Academics.Queries.Events.ScheduleEvents
{
    public record ScheduleEventsResponse
    (
        List<EventsListDetails> eventsList
        );
        public record EventsListDetails(
        Dictionary<string, EventsDetails> eventsDetails
            );
        public record EventsDetails(
            string id="",
            string title = "",
            string? descriptions = "",
            EventType? eventsType = default,
            string eventsDate = "",
            string participants = "",
            TimeOnly eventTime = default,
            string venue = "",
            string? chiefGuest = "",
            string? organizer = "",
            string? mentor = ""
        );
    
}
