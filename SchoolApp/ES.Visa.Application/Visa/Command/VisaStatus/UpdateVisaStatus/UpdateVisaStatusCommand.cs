using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Enum.VisaEnum;

namespace ES.Visa.Application.Visa.Command.VisaStatus.UpdateVisaStatus
{
    public record UpdateVisaStatusCommand
    (
        string id,
            string Name,
        VisaStatusType VisaStatusType
        ) :IRequest<Result<UpdateVisaStatusResponse>>;
}
