using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddSeatPlanning.RequestCommandMapper
{
    public static class AddSeatPlanningRequestMapper
    {
        public static AddSeatPlanningCommand ToCommand(this AddSeatPlanningRequest request)
        {
            return new AddSeatPlanningCommand(
                request.ExamSessionId,
                request.classIds
                );
        }
    }
}
