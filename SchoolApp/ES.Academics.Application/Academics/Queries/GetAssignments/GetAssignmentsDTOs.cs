using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.GetAssignments
{
    public record GetAssignmentsDTOs
    (
        string? classId,
        string? subjectId
        );
}
