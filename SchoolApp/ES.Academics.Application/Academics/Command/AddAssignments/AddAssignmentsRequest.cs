using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddAssignments
{
    public record AddAssignmentsRequest
    (
         string title,
            string description,
            DateTime dueDate,
            string? classId,
            string? subjectId
        );
}
