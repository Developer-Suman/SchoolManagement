using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;

namespace ES.Student.Application.CocurricularActivities.Command.Participation.UpdateParticipation
{
    public record UpdateParticipationResponse
    (
            string studentId="",
            string activityId="",
            AwardPosition awardPosition=default,
            string? certificateTitle="",
            string? certificateContent=""
            
        );
}
