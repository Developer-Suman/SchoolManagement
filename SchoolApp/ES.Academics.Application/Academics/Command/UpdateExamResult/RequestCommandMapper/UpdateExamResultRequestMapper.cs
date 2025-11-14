using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.UpdateExamResult.RequestCommandMapper
{
    public static class UpdateExamResultRequestMapper
    {
        public static UpdateExamResultCommand ToCommand(this UpdateExamResultRequest request, string examResultId)
        {
            return new UpdateExamResultCommand
                (
                examResultId,
                request.examId,
                request.studentId,

                request.remarks,
                request.marksObtained
                );
        }
    }
}
