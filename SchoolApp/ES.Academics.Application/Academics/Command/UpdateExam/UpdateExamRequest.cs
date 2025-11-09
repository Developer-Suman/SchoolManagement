using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.UpdateExam
{
    public record UpdateExamRequest
    (
        string name,
        DateTime examDate,
        decimal totalMarks,
        decimal passingMarks,
        bool? isfinalExam
        );
}
