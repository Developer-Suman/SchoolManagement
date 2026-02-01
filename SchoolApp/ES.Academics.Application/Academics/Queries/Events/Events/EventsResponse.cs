using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.Events.Events
{
    public record EventsResponse
    (

        string id="",
            string title="",
            string? descriptions="",
            string eventsType="",
            string eventsDate="",
            string participants = "",
            string eventTime="",
            string venue = "",
            string? chiefGuest = "",
            string? organizer = "",
            string? mentor = "",
            string schoolId = "",
            string createdBy = "",
            string createdAt="",
            string modifiedBy = "",
            string modifiedAt = "",
            bool isActive=false
        );
}
