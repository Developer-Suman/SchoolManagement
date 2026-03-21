using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.Events.ScheduleEvents
{
    public record ScheduleEventsDTOs
    (
        string? title,
        string? startDate,
        string? endDate
        );
}
