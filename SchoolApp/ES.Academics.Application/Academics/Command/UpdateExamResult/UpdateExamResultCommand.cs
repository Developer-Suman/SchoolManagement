using ES.Academics.Application.Academics.Command.AddExamResult;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;

namespace ES.Academics.Application.Academics.Command.UpdateExamResult
{
    public record UpdateExamResultCommand
    (
        string id,
             string? examId,
            string studentId,
            string remarks,
            List<MarksObtainedDTOs> marksObtained
        ) : IRequest<Result<UpdateExamResultResponse>>;
}
