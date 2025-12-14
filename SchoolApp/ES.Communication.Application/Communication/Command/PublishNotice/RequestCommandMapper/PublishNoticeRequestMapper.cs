using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Communication.Application.Communication.Command.PublishNotice.RequestCommandMapper
{
    public static class PublishNoticeRequestMapper
    {
        public static PublishNoticeCommand ToCommand(this PublishNoticeRequest request)
        {
            return new PublishNoticeCommand
                (
                request.noticeId
                );
        }
    }
}
