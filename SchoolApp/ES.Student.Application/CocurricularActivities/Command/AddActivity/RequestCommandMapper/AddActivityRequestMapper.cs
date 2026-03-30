using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.CocurricularActivities.Command.AddActivity.RequestCommandMapper
{
    public static class AddActivityRequestMapper
    {
        public static AddActivityCommand ToCommand(this AddActivityRequest request)
        {
            return new AddActivityCommand(
                request.name,
                request.descriptions,
                request.activityCategory,
                request.eventId,
                request.startTime,
                request.endTime,
                request.activityDate,
                request.classIds
                );
        }
    }
}
