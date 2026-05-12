using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.FollowUp.UpdateFollowUp.RequestCommandMapper
{
    public static class UpdateFollowUpRequestMapper
    {
        public static UpdateFollowUpCommand ToCommand(this UpdateFollowUpRequest request, string id)
        {
            return new UpdateFollowUpCommand(
                id,
                request.startTime,
                request.endTime,
                request.followUpDate,
                request.notes,
                request.followUpStatus,
                request.appointmentId
                );
        }
    }
}
