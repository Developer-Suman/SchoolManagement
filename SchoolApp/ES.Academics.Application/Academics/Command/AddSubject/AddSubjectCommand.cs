using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.AddSubject
{
    public record AddSubjectCommand
    (
         string name,
            string code,
            int? creditHours,
            string? description,
            string classId,
                string examId,
            int fullMarks,
            int passMarks
        ) : IRequest<Result<AddSubjectResponse>>;
}
