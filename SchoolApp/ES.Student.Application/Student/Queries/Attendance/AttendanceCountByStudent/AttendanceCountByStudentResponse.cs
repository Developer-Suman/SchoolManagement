using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Student.Queries.Attendance.AttendanceCountByStudent
{
    public record AttendanceCountByStudentResponse
    (
        int? totalRunningDays=0,
        int? totalPresentDays=0,
        int? totalAbsentDays=0,
        int? totalLateDays=0,
        int? totalExcusedDays =0

        );
}
