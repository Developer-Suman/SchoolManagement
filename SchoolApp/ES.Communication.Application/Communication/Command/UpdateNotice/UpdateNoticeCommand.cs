using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Communication.Application.Communication.Command.UpdateNotice
{
    public record UpdateNoticeCommand
    (
        string id,
        string title,
        string contentHtml,
        string? shortDescription,
        string modifiedBy,
        string modifiedAt
    ) : IRequest<Result<UpdateNoticeResponse>>;
}
