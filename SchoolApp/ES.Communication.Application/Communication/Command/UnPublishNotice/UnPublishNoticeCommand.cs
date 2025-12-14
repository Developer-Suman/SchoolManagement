using ES.Communication.Application.Communication.Command.PublishNotice;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Communication.Application.Communication.Command.UnPublishNotice
{
    public record UnPublishNoticeCommand
      (
        string noticeId
        ) : IRequest<Result<UnPublishNoticeResponse>>;
}
