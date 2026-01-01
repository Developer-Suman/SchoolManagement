using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.FilterSubject
{
    public record FilterSubjectResponse
    (
        string id,
            string name,
            string code,
            int? creditHours,
            string? description,
            string classId,

            string schoolId,
            bool isActive,
                string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
        );
}
