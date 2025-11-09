using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.Student.Command.DeleteParent
{
    public record DeleteParentCommand
    (
        string id
    ):IRequest<Result<bool>>;
}
