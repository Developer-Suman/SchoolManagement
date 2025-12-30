using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.EvaluteAssignments
{
    public class EvaluteAssignmentCommandValidator : AbstractValidator<EvaluteAssignmentCommand>
    {
        public EvaluteAssignmentCommandValidator()
        {
            RuleFor(x => x.assignmentId)
             .NotEmpty()
             .WithMessage("Assignment ID is required.");
            RuleFor(x => x.studentId)
             .NotEmpty()
             .WithMessage("Student ID is required.");


        }
    }
}
