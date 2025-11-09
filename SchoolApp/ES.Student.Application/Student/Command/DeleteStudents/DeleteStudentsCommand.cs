using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.Student.Command.DeleteStudents
{
    public record  DeleteStudentsCommand
    (string id):IRequest<Result<bool>>;
}
