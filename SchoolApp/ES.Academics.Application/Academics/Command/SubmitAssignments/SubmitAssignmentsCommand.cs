using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.SubmitAssignments
{
    public record SubmitAssignmentsCommand
    (
         string assignmentId,
            string submissionText,
            IFormFile? submissionFile
        ) : IRequest<Result<SubmitAssignmentsResponse>>;
}
