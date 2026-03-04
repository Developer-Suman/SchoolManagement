using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.ExamById
{
    public record ExamByIdQueryResponse
    (
        string id="",
         string name="",
         DateTime examDate= default,
         decimal totalMarks=0,
         decimal passingMarks=0,
         bool? isfinalExam=true,
         string classId = ""
        );
}
