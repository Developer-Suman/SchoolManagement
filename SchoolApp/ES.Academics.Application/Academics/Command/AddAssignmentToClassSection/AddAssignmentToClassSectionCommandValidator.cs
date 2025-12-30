using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddAssignmentToClassSection
{
    public class AddAssignmentToClassSectionCommandValidator: AbstractValidator<AddAssignmentToClassSectionCommand>
    {
        public AddAssignmentToClassSectionCommandValidator()
        {
            RuleFor(x => x.assignmentId)
            .NotEmpty()
            .WithMessage("AssignmentId is required.");
   
        }
    }
}
