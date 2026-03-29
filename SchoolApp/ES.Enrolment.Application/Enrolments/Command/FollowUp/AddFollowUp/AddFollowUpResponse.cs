using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Enrolment.Application.Enrolments.Command.FollowUp.AddFollowUp
{
    public record AddFollowUpResponse
    (
         string id="",
            TimeOnly startTime=default,
            TimeOnly endTime=default,
            DateTime followUpDate=default,
            string notes = "",
            FollowUpStatus followUpStatus = default,
            bool isActive=true,
            string schoolId = "",
            string createdBy = "",
            DateTime createdAt = default,
            string modifiedBy = "",
            DateTime modifiedAt = default
        );
}
