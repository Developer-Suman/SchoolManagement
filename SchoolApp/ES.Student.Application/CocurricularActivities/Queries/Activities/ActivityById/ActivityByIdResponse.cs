using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;

namespace ES.Student.Application.CocurricularActivities.Queries.Activities.ActivityById
{
    public record ActivityByIdResponse
    (
         string id = "",
            string name = "",
            string? descriptions = "",
            ActivityCategory activityCategory = default,
            string eventId = "",
            TimeOnly startTime = default,
            TimeOnly endTime = default,
            string activityDate = "",
            bool isActive = true,
            string schoolId = "",
            string createdBy = "",
            DateTime createdAt = default,
            string modifiedBy = "",
            DateTime modifiedAt = default
        );
}
