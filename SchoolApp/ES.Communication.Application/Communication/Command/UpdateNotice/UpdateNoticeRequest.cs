using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Communication.Application.Communication.Command.UpdateNotice
{
    public record UpdateNoticeRequest
    (
        string title,
        string contentHtml,
        string? shortDescription
    );
}
