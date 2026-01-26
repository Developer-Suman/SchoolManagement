using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Student.Application.Student.Queries.Attendance.AttendanceReport
{
    public record AttendanceReportDTOs
    (
        string? startDate,
        string? endDate,
        string? academicTeamId,
        string? classId,
        string? yearName,
        NameOfMonths? nameOfMonths
        );
}
