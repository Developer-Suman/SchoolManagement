using ES.Communication.Application.Communication.Command.PublishNotice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Communication.Application.Communication.Command.UnPublishNotice.RequestCommandMapper
{
    public static class UnPublishNoticeRequestMapper
    {
        public static UnPublishNoticeCommand ToCommand(this UnPublishNoticeRequest request)
        {
            return new UnPublishNoticeCommand
                (
                request.noticeId
                );
        }
    }
}
