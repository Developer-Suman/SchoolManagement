using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Communication.Application.Communication.Command.UpdateNotice.RequestCommandMapper
{
    public static class UpdateNoticeRequestMapper
    {
        public static UpdateNoticeCommand ToCommand(this UpdateNoticeRequest request, string Id)
        {
            return new UpdateNoticeCommand(
                Id,
                request.title,
                request.contentHtml,
                request.shortDescription,
                request.modifiedBy,
                request.modifiedAt
                 );
        }
    }
}
