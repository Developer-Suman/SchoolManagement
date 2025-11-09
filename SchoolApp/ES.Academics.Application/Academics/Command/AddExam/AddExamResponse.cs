using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddExam
{
    public record AddExamResponse
    (
        string name,
            DateTime examDate,
            decimal totalMarks,
            decimal passingMarks,
            bool? isfinalExam
        );
}
