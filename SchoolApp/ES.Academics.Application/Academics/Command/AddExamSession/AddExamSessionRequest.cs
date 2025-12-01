using ES.Academics.Application.Academics.Command.AddExamResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddExamSession
{
    public record AddExamSessionRequest
    (
        string name,
        DateTime examDate,
        List<ExamHallDTOs> ExamHallDTOs
        );
}
