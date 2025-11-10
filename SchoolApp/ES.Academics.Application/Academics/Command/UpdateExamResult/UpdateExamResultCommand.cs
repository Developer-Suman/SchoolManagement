using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.UpdateExamResult
{
    public record UpdateExamResultCommand
    (
        string id,
         string? examId,
     string studentId,
     string subjectId,
     decimal marksObtained,
     string grade,
     string remarks,
     bool isActive,
     string schoolId
        ) : IRequest<Result<UpdateExamResultResponse>>;
}
