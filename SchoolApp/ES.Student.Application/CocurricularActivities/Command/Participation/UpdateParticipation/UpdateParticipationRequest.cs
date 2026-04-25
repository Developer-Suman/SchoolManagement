using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;

namespace ES.Student.Application.CocurricularActivities.Command.Participation.UpdateParticipation
{
    public record UpdateParticipationRequest
    (
        string id,
            string studentId,
            string activityId,
            AwardPosition awardPosition,
            string? certificateTitle,
            string? certificateContent
        );
}
