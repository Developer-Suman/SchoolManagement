using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Registration.Command.RegisterMultipleStudents.RequestCommandMapper
{
    public static class RegisterMultipleStudentsRequestMapper
    {
        public static RegisterMultipleStudentsCommand ToCommand(this RegisterMultipleStudentsRequest request)
        {
            return new RegisterMultipleStudentsCommand(
                request.studentIds,
                request.classId,
                request.academicYearId
                );
        }
    }
}
