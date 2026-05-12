using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Enrolment.Application.Enrolments.Queries.FollowUp.FollowUpId
{
    public record FollowUpIdResponse
   (
        string id = "",
            string userId = "",
            TimeOnly startTime = default,
            TimeOnly endTime = default,
            DateTime followUpDate = default,
            string notes = "",
            FollowUpStatus followUpStatus = default,
            string appointmentId = "",
            bool isActive = true,
            string schoolId = "",
            string createdBy = "",
            DateTime createdAt = default,
            string modifiedBy = "",
            DateTime modifiedAt = default
        );
}
