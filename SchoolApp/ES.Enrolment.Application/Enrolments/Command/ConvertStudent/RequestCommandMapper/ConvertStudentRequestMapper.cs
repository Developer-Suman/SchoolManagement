using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.ConvertStudent.RequestCommandMapper
{
    public static class ConvertStudentRequestMapper
    {
        public static ConvertStudentCommand ToCommand(this ConvertStudentRequest request)
        {
            return new ConvertStudentCommand
                (
                request.userId,
                request.universityName,
                request.visaId
                );
        }
    }
}
