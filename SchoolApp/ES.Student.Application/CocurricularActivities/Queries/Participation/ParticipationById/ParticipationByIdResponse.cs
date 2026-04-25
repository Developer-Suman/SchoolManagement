using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;

namespace ES.Student.Application.CocurricularActivities.Queries.Participation.ParticipationById
{
    public record ParticipationByIdResponse
    (
         string id = "",
            string studentId = "",
            string activityId = "",
            AwardPosition awardPosition = default,
            string? certificateTitle = "",
            string? certificateContent = "",
            bool isActive = true,
            string schoolId = "",
            string createdBy = "",
            DateTime createdAt = default,
            string modifiedBy = "",
            DateTime modifiedAt = default
        );
}
