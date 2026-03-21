using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Queries.Events.ScheduleEvents
{
    public record ScheduleEventsQuery
    (
        ScheduleEventsDTOs scheduleEventsDTOs
        ) : IRequest<Result<ScheduleEventsResponse>>;
}
