using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Communication.Application.Communication.Command.AddNotice.RequestCommandMapper
{
    public static class AddNoticeRequestMapper
    {
        public static AddNoticeCommand ToCommand(this AddnoticeRequest request)
        {
            return new AddNoticeCommand
                (
                request.title,
                request.contentHtml,
                request.shortDescription
                );
        }
    }
}
