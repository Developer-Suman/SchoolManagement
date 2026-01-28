using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.Events.UpdateEvents
{
    public record UpdateEventsResponse
    (
         string id="",
            string title="",
            string? descriptions="",
            string eventsType="",
            string eventsDate="",
            string participants="",
            string eventTime = "",
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
