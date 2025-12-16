using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.Student.Queries.GetStudentForAttendance
{
    public record StudentForAttendanceQuery
    (
        ): IRequest<Result<List<StudentForAttendanceResponse>>>;
}
