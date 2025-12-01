using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddExamSession
{
    public record AddExamSessionResponse
    (
        string name="",
        DateTime examDate=default,
        List<ExamHallDTOs> ExamHallDTOs = default
        );
}
