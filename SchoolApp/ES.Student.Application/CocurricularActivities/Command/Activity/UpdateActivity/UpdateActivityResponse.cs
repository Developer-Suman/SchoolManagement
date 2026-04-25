using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;

namespace ES.Student.Application.CocurricularActivities.Command.Activity.UpdateActivity
{
    public record UpdateActivityResponse
    (
            string name="",
            string? descriptions="",
            ActivityCategory activityCategory=default,
            string eventId="",
            TimeOnly startTime=default,
            TimeOnly endTime=default,
            string activityDate=""
        );
}
