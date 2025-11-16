using ES.Academics.Application.Academics.Command.AddExamResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.MarkSheetByStudent
{
    public record MarkSheetByStudentResponse
    (
         string? examId="",
     string studentId="",
     string remarks="",
     bool isActive=true,
     string schoolId="",
        string createdBy="",
     DateTime createdAt=default,
     string modifiedBy="",
     DateTime modifiedAt=default,
     string percentage="",
     decimal totalObtainedMarks = 0,
     string grade="",
     string division="",

     List<MarksObtainedDTOs> marksObtained=default
        );
}
