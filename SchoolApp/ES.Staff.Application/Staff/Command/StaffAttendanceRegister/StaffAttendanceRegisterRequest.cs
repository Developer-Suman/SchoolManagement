using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.Staff.Command.StaffAttendanceRegister
{
    public record StaffAttendanceRegisterRequest
    (
            string userId,
            string token
        );
}
