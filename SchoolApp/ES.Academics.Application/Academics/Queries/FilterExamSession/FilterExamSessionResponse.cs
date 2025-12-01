using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.FilterExamSession
{
    public record FilterExamSessionResponse
    (
         string id,
            string name,
            DateTime date,
                   string schoolId,
            bool isActive,
             string createdBy,
                  DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
        );
}
