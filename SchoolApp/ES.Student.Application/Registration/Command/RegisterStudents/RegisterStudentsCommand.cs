using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.Registration.Command.RegisterStudents
{
    public record RegisterStudentsCommand
    (
         string studentId,
        string classId,
        string academicYearId
        ): IRequest<Result<RegisterStudentsResponse>>;
}
