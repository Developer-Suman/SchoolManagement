using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddAssignmentStudents
{
    public class AddAssignmentStudentsCommandValidator : AbstractValidator<AddAssignmentStudentsCommand>
    {
        public AddAssignmentStudentsCommandValidator()
        {
            RuleFor(x => x.assignmentId)
             .NotEmpty()
             .WithMessage("Assigment is required.");
           
        }
    }
}
