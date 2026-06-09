using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Enrolment.Application.Enrolments.Command.FollowUp.UpdateFollowUp
{
    public record UpdateFollowUpRequest
    (

        TimeOnly startTime,
            TimeOnly endTime,
            string followUpDate,
            string notes,
            FollowUpStatus followUpStatus,
            string appointmentId
        );
}
