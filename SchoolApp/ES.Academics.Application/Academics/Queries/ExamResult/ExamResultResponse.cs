using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.ExamResult
{
    public record ExamResultResponse
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
