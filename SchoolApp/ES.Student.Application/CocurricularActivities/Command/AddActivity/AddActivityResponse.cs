using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;

namespace ES.Student.Application.CocurricularActivities.Command.AddActivity
{
    public record AddActivityResponse
    (
        string id="",
            string name="",
            ActivityCategory activityCategory=default,
            string eventId = "",
            bool isActive=true,
            string schoolId = "",
            string createdBy = "",
            DateTime createdAt=default,
            string modifiedBy = "",
            DateTime modifiedAt = default
        );
}
