using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.EvaluteAssignments
{
    public record EvaluteAssignmentCommand
    (
          string assignmentId,
        string studentId,
        decimal marks,
        string? teacherRemark
        ) : IRequest<Result<EvaluteAssignmentsResponse>>;
}
