using ES.Communication.Application.Communication.Command.PublishNotice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Communication.Application.Communication.Queries.FilterNotice
{
    public record FilterNoticeResponse
    (
        string id,
            string title,
            string contentHtml,
            string? shortDescription,
            DateTime createdAt,
            PublishStatus publishStatus,
            string createdBy,
            DateTime modifiedAt,
            string modifiedBy,
            string schoolId
        );
}
