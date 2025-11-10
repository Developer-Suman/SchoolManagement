using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.AddExamResult
{
    public record AddExamResultCommand
    (
        string? examId,
            string studentId,
            string subjectId,
            decimal marksObtained,
            string grade,
            string remarks,
            string schoolId
        ) : IRequest<Result<AddExamResultResponse>>;
}
