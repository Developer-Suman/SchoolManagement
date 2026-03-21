using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;

namespace ES.Student.Application.Student.Queries.ActivityByEvents
{
    public record ActivityByEventsResponse
    (
        string title="",
            string? descriptions="",
            EventType? eventsType=default,
            string eventsDate = "",
            TimeOnly? eventTime=default,
            string venue = "",
            string? chiefGuest = "",
            string? organizer = "",
            string? mentor = "",
            string ActivityName = "",
            ActivityCategory activityCategory = default,
            TimeOnly startTime = default,
            TimeOnly endTime = default,
            string activityDate=""
        );
}
