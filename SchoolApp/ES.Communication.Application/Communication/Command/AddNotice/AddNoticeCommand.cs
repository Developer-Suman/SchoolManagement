using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Communication.Application.Communication.Command.AddNotice
{
    public record AddNoticeCommand
    (
        string title,
            string contentHtml,
            string? shortDescription
        ) : IRequest<Result<AddNoticeResponse>>;
}
