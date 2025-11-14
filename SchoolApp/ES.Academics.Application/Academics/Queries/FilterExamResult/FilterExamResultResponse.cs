using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.FilterExamResult
{
    public record FilterExamResultResponse
    (
        string id,
        string? examId,
     string studentId,
     decimal marksObtained,
     string remarks,
     bool isActive,
     string schoolId,
     string createdBy,
     DateTime createdAt,
     string modifiedBy,
     DateTime modifiedAt
        );
}
