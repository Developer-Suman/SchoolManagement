using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddExam
{
    public record ExamSubjectDTOs
    (
            string subjectId,
            int passMarks,
            int fullMarks
        );
}
