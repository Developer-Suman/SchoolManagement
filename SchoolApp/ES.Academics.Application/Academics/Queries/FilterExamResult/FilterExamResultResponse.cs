using ES.Academics.Application.Academics.Command.AddExamResult;
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
        string? studentId,
     string remarks,
     bool isActive,
     string schoolId,
     string createdBy,
     DateTime createdAt,
     string modifiedBy,
     DateTime modifiedAt,
     List<MarksObtainedDTOs> marksObtained
        );
}
