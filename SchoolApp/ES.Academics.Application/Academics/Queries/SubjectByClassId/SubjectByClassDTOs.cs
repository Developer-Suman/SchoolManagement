using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.SubjectByClassId
{
    public record SubjectByClassDTOs
    (
        string? examId,
        string classId
        );
}
