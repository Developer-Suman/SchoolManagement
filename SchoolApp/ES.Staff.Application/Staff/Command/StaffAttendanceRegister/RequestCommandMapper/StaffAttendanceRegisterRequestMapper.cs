using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.Staff.Command.StaffAttendanceRegister.RequestCommandMapper
{
    public static class StaffAttendanceRegisterRequestMapper
    {
        public static StaffAttendanceregisterCommand ToCommand(this StaffAttendanceRegisterRequest request)
        {

            return new StaffAttendanceregisterCommand
                (
                request.userId,
                request.token
                );
            
        }
    }
}
