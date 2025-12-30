using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.SubmitAssignments
{
    public class SubmitAssignmentsCommandValidator : AbstractValidator<SubmitAssignmentsCommand>
    {
        public SubmitAssignmentsCommandValidator()
        {
            RuleFor(x => x.assignmentId)
             .NotEmpty()
             .WithMessage("Assignment ID is required.");
            RuleFor(x => x.submissionText)
             .MaximumLength(1000)
             .WithMessage("Submission text cannot exceed 1000 characters.");
        }
    }
}
