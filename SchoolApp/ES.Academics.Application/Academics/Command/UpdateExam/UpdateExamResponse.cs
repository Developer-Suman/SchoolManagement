using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.UpdateExam
{
    public record UpdateExamResponse
    (
              string name,
        DateTime examDate,
        bool? isfinalExam,
        string classId
        );
}
