using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;

namespace ES.Student.Application.Student.Command.AddAttendances
{
    public record StudentAttendanceDto
    (
        string studentId,
        AttendanceStatus status,
        string? remarks
        );
}
