using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddExamResult.RequestCommandMapper
{
    public static class AddExamResultRequestMapper
    {
        public static AddExamResultCommand ToCommand(this AddExamResultRequest request)
        {
            return new AddExamResultCommand(
                request.examId,
                request.studentId,
                request.subjectId,
                request.marksObtained,
                request.grade,
                request.remarks,
                request.schoolId
                );
        }
    }
}
