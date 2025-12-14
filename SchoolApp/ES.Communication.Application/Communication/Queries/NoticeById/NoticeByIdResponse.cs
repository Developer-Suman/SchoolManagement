using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Communication.Application.Communication.Queries.NoticeById
{
    public record NoticeByIdResponse
   (
        string id,
            string title,
            string contentHtml,
            string? shortDescription,
            DateTime createdAt,
            string createdBy,
            DateTime modifiedAt,
            string modifiedBy,
            string schoolId
        );
}
