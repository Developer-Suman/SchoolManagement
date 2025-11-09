using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Student.Command.AddStudents
{
    public class AddStudentsCommandValidator : AbstractValidator<AddStudentsCommand>
    {
        public AddStudentsCommandValidator()
        {
            RuleFor(x => x.firstName)
               .NotEmpty().WithMessage("First name is required.");
        }
    }
}
