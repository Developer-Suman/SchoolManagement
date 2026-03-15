using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Command.TranningRegistration.AddTranningRegistration
{
    public record AddTranningRegistrationCommand
    (
          string applicantId,
        string consultancyClassId,
        DateTime registeredAt
        ) : IRequest<Result<AddTranningRegistrationResponse>>;
}
