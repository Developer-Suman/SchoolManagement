using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.SubmitAssignments
{
    public record SubmitAssignmentsResponse
    (
        string id,
            string assignmentId,
            string studentId,
            bool isSubmitted,
            DateTime? submittedAt,
            string? submissionText,
            string? submissionFileUrl,
            decimal? marks,
            string? teacherRemarks,
             bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime? modifiedAt
        );
}
