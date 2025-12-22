using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddExam.RequestCommandMapper
{
    public static class AddExamRequestMapper
    {
        public static AddExamCommand ToCommand(this AddExamRequest request)
        {
            return new AddExamCommand(
                request.name,
                request.examDate,
                request.isfinalExam,
                request.classId
                );
        }
    }
}
