using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;

namespace ES.Student.Application.CocurricularActivities.Command.Addparticipation
{
    public record AddParticipationRequest
    (
        string studentId,
            string activityId,
            AwardPosition awardPosition
        );
}
