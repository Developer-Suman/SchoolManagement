using ES.Enrolment.Application.Enrolments.Command.ConsultancyClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.AddConsultancyClass.RequestCommandMapper
{
    public static class AddConsultancyClassRequestMapper
    {
        public static AddConsultancyClassCommand ToCommand(this AddConsultancyClassRequest request)
        {
            return new AddConsultancyClassCommand(
                request.name,
                request.startTime,
                request.endTime,
                request.batch,
                request.englishProficiency
            );
        }
    }
}
