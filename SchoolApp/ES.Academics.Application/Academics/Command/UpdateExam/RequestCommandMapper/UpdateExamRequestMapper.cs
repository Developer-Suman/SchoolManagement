using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.UpdateExam.RequestCommandMapper
{
    public static class UpdateExamRequestMapper
    {
        public static UpdateExamCommand ToCommand(this UpdateExamRequest request, string examId)
        {
            return new UpdateExamCommand(
                examId,
                request.name,
                request.examDate,
                request.totalMarks,
                request.passingMarks,
                request.isfinalExam,
                request.classId
            );
        }
    }
}
