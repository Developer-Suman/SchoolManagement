using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.TranningRegistration.AddTranningRegistration
{
    public record AddTranningRegistrationRequest
    (
        string applicantId,
        string consultancyClassId,
        DateTime registeredAt
        );
}
