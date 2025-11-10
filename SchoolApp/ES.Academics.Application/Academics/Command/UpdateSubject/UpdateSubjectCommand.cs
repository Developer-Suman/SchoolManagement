using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.UpdateSubject
{
    public record UpdateSubjectCommand
    (
        string id,
        string name,
            string code,
            int? creditHours,
            string? description,
            string classId
        ): IRequest<Result<UpdateSubjectResponse>>;
}
