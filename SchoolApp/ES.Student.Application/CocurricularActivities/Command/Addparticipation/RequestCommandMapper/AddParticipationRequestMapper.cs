using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.CocurricularActivities.Command.Addparticipation.RequestCommandMapper
{
    public static class AddParticipationRequestMapper
    {
            public static AddParticipationCommand ToCommand(this AddParticipationRequest request)
            {
            return new AddParticipationCommand
            (
                request.studentId,
                request.activityId,
                request.awardPosition,
                request.certificateTitle,
                request.certificateContent
                );
                
        }
    }
}
