using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.AddAssignmentStudents
{
    public record AddAssignmentStudentsCommand
    (
         string assignmentId,
            string studentId,
            bool isSubmitted,
            DateTime? submittedAt,
            decimal? marks
        ) : IRequest<Result<AddAssignmentStudentsResponse>>;
}
