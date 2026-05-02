using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Visa.Application.Visa.Command.VisaStatus.DeleteVisaStatus
{
    public record DeleteVisaStatusCommand
    (
        string id
    ) : IRequest<Result<bool>>;
}
