using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.Student.Queries.Attendance.AttendanceCountByStudent
{
    public record AttendanceCountByStudentQuery
    (
        AttendanceCountByStudentsDTOs AttendanceCountByStudentsDTOs
        ) : IRequest<Result<AttendanceCountByStudentResponse>>;
}
