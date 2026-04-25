using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.CocurricularActivities.Command.Activity.UpdateActivity.RequestCommandMapper
{
    public static class UpdateActivityRequestMapper
    {
        public static UpdateActivityCommand ToCommand(this UpdateActivityRequest request, string id)
        {
            return new UpdateActivityCommand(
                id,
                request.name,
                request.descriptions,
                request.activityCategory,
                request.eventId,
                request.startTime,
                request.endTime,
                request.activityDate
            );
        }
    }
}
