using ES.Academics.Application.Academics.Command.AddExam;
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
         string classId="",
         List<ExamSubjectDTOs> ExamSubjectDTOs = default
        );
}
