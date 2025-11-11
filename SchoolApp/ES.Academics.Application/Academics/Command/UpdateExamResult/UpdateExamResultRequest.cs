using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.UpdateExamResult
{
    public record UpdateExamResultRequest
    (
         string? examId,
            string studentId,
            string subjectId,
            decimal marksObtained,
            string grade,
            string remarks
        );
}
