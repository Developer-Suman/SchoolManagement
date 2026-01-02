using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.AddAssignments
{
    public record AddAssignmentsCommand
    (
         string title,
            string description,
            DateTime dueDate,
            string? classId,
            string? subjectId
        ) : IRequest<Result<AddAssignmentsResponse>>;
}
