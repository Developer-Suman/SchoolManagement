using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;

namespace ES.Student.Application.CocurricularActivities.Command.Activity.AddActivity
{
    public record AddActivityRequest
    (
        string name,
        string? descriptions,
            ActivityCategory activityCategory,
            string eventId,
            TimeOnly startTime,
            TimeOnly endTime,
            string activityDate,
            List<string>? classIds
        );
}
