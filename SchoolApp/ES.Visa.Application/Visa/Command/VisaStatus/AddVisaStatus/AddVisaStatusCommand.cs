using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Enum.VisaEnum;

namespace ES.Visa.Application.Visa.Command.VisaStatus.AddVisaStatus
{
    public record AddVisaStatusCommand
    (
        string name,
            VisaStatusType visaStatusType
        ) : IRequest<Result<AddVisaStatusResponse>>;
}
