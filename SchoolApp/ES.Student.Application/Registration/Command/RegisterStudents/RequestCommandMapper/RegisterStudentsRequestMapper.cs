using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Registration.Command.RegisterStudents.RequestCommandMapper
{
    public static class RegisterStudentsRequestMapper
    {
        public static RegisterStudentsCommand ToCommand(this RegisterStudentsRequest request)
        {
            return new RegisterStudentsCommand(
                request.studentId,
                request.classId,
                request.academicYearId
                );
        }
    }
}
