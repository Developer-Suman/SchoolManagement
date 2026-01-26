using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Students;

namespace ES.Student.Application.Student.Command.AddAttendances
{
    public record AddAttendenceCommand
    (
        string classId,
        List<StudentAttendanceDto> StudentAttendances
        ) : IRequest<Result<List<AddAttendanceResponse>>>;
}
