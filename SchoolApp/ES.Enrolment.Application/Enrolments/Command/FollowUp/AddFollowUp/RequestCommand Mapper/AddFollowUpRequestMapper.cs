using ES.Enrolment.Application.Enrolments.Command.FollowUp.AddFollowUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.AddFollowUp.RequestCommand_Mapper
{
    public static class AddFollowUpRequestMapper
    {
        public static AddFollowUpCommand ToCommand(this AddFollowUpRequest request)
        {
            return new AddFollowUpCommand
                (
                request.leadId,
                request.startTime,
                request.endTime,
                request.followUpDate,
                request.notes,
                request.followUpStatus
                );
        }
    }
}
