using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Communication.Application.Communication.Command.PublishNotice
{
    public record PublishNoticeResponse
    (
        string noticeid,
        PublishStatus publishStatus
        );

    public enum PublishStatus
    {
        Published,
        UnPublished
    }
}
