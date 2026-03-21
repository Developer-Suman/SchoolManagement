using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;

namespace ES.Academics.Application.Academics.Command.Events.UpdateEvents
{
    public record UpdateEventsResponse
    (
         string id="",
            string title="",
            string? descriptions="",
            EventType? eventsType =default,
            string eventsDate="",
            string participants="",
            TimeOnly eventTime = default,
            string venue="",
            string? chiefGuest="",
            string? organizer="",
            string? mentor="",
            string schoolId="",
            string createdBy="",
            DateTime createdAt=default,
            string modifiedBy="",
            DateTime modifiedAt=default,
            bool isActive=false
        );
}
