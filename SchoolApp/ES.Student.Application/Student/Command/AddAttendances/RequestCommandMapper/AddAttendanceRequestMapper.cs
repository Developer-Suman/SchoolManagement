using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Student.Command.AddAttendances.RequestCommandMapper
{
    public static class AddAttendanceRequestMapper
    {
        public static AddAttendenceCommand ToCommand(this AddAttendanceRequest request)
        {
            return new AddAttendenceCommand(
                request.classId,
                request.StudentAttendances
                );
        }
    }
}
