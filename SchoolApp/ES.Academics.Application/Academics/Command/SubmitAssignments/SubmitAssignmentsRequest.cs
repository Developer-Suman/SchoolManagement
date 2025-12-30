using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.SubmitAssignments
{
    public record SubmitAssignmentsRequest
    (
         string assignmentId,
            string submissionText,
            IFormFile? submissionFile
        );
}
