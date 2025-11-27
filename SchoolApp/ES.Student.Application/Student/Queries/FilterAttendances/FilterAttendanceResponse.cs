using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;

namespace ES.Student.Application.Student.Queries.FilterAttendances
{
    public record FilterAttendanceResponse
   (
         string id,
            string studentId,
            DateTime attendanceDate,
            AttendanceStatus attendanceStatus,
            string academicTeamId,
            string? remarks,
            DateTime createdAt
        );
}
