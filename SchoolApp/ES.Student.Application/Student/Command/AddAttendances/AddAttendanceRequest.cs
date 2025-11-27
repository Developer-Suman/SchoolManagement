using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;

namespace ES.Student.Application.Student.Command.AddAttendances
{
    public record AddAttendanceRequest
    (
        string AcademicTeamId,
        DateTime AttendanceDate,
        List<StudentAttendanceDto> StudentAttendances

        );
}
