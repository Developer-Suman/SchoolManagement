using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.GetAssignments
{
    public record GetAssignmentsResponse
    (
         string id,
            string title,
            string description,
            DateTime dueDate,
            string academicTeamId,
            string? classId,
            string? subjectId,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime? modifiedAt
        );
}
