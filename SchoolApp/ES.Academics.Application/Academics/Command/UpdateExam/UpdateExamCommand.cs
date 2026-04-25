using ES.Academics.Application.Academics.Command.AddExam;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.UpdateExam
{
    public record UpdateExamCommand
    (
        string id,
              string name,
        DateTime examDate,
        bool? isfinalExam,
        string classId,
        List<UpdateExamSubjectDTOs?> UpdateExamSubjectDTOs = default
        ) : IRequest<Result<UpdateExamResponse>>;
}
