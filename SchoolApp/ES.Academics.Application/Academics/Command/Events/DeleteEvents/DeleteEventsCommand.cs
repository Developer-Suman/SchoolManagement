using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using ZXing;

namespace ES.Academics.Application.Academics.Command.Events.DeleteEvents
{
    public record DeleteEventsCommand
    (
        string Id
        ): IRequest<Result<bool>>;
}
