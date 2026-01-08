using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.Exam
{
    public record ExamQueryResponse
    (
        string id="",
                 string name="",
            DateTime examDate=default,
            decimal totalMarks=0,
            bool? isfinalExam=true,
            string classId = ""
        );
}
