using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Visa.Application.Visa.Command.VisaApplication.DeleteVisaApplication
{
    public record DeleteVisaApplicationCommand
    (
        string id
        ) : IRequest<Result<bool>>;
}
