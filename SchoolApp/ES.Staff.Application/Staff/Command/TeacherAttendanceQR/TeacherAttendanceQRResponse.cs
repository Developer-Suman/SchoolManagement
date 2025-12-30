using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.Staff.Command.TeacherAttendanceQR
{
    public record TeacherAttendanceQRResponse
    (
        string id,
        string token,
        string qrCodeImageUrl

        );
}
