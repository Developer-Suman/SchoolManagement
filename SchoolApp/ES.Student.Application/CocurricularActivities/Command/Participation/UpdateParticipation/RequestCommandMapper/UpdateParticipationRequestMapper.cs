using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.CocurricularActivities.Command.Participation.UpdateParticipation.RequestCommandMapper
{
    public static class UpdateParticipationRequestMapper
    {
        public static UpdateParticipationCommand ToCommand(this UpdateParticipationRequest request, string id)
        {
            return new UpdateParticipationCommand(
                id,
                request.studentId,
                request.activityId,
                request.awardPosition,
                request.certificateTitle,
                request.certificateContent
            );
        }
    }
}
