using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddExamSession.RequestCommandMapper
{
    public static class AdSessionRequestMapper
    {
        public static AddExamSessionCommand ToCommand(this AddExamSessionRequest request)
        {
            return new AddExamSessionCommand(
                request.name,
                request.examDate,
                request.ExamHallDTOs.Select(e => new ExamHallDTOs(
                    e.hallName,
                    e.capacity)).ToList()
                );
        }
    }
}
