using ES.Communication.Application.Communication.Command.PublishNotice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Communication.Application.Communication.Command.UnPublishNotice
{
    public record UnPublishNoticeResponse
    (
        string noticeid,
        PublishStatus publishStatus
        );
}
