using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Enrolment.Application.Enrolments.Queries.FollowUp.FilterFollowUp
{
    public record FilterFollowUpResponse
    (
        string id = "",
            string userId = "",
            string fullName="",
            TimeOnly startTime = default,
            TimeOnly endTime = default,
            string followUpDate = "",
            string notes = "",
            FollowUpStatus followUpStatus = default,
            string appointmentId = "",  
            string CounserlorWithDate="",
            bool isActive = true,
            string schoolId = "",
            string createdBy = "",
            DateTime createdAt = default,
            string modifiedBy = "",
            DateTime modifiedAt = default
        );
}
