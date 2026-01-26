using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;

namespace ES.Student.Application.Student.Queries.Attendance.AttendanceReport
{
    public record AttendanceReportResponse(
     string ClassId,
    string AcademicTeamId,
    List<AttendanceStudentDetail> Students
 );

    public record AttendanceStudentDetail(
    string StudentId,
    Dictionary<string, AttendanceDetail> Attendance
);

    public record AttendanceDetail(
        string Status,
        string? Review
    );
}
