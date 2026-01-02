using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.SchoolClass
{
    public record SubjectDTOs
    (
        string id,
            string name,
            string code,
            int? creditHours,
            string? description,
            string classId
        );
}
