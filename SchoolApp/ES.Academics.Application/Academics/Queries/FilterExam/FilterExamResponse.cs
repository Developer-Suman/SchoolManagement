using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.FilterExam
{
    public record FilterExamResponse
    (
        string id,
         string name,
            DateTime examDate,
            decimal totalMarks,

            bool? isfinalExam,
            string classId
        );
}
