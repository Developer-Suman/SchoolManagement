using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.TranningRegistration.AddTranningRegistration.RequestCommandMapper
{
    public static class AddTranningRegistrationRequestMapper
    {
        public static AddTranningRegistrationCommand ToCommand(this AddTranningRegistrationRequest request)
        {
            return new AddTranningRegistrationCommand(
                request.applicantId,
                request.consultancyClassId,
                request.registeredAt
                );
        }
    }
}
