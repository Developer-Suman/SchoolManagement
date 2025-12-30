using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.EvaluteAssignments
{
    public record EvaluteAssignmentsResponse
    (
        string id,
            string assignmentId,
            string studentId,
            bool isSubmitted,
            DateTime? submittedAt,
            string? submissionText,
            string? submissionFileUrl,
            decimal? marks,
            string? teacherRemarks
        );
}
