using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.UpdateExamResult
{
    public record UpdateExamResultResponse
   (
         string? examId,
            string studentId,
            string subjectId,
            decimal marksObtained,
            string grade,
            string remarks,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
        );
}
