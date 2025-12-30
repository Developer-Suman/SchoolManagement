using ES.Staff.Application.Staff.Command.TeacherAttendanceQR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Staff.Application.ServiceInterface
{
    public interface ITeacherAttendanceServices
    {
        Task<Result<TeacherAttendanceQRResponse>> CreateQR(TeacherAttendanceQRCommand teacherAttendanceQRCommand);
    }
}
