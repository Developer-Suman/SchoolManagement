using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Registration.Command.RegisterMultipleStudents
{
    public class RegisterMultipleStudentsCommandValidator : AbstractValidator<RegisterMultipleStudentsCommand>
    {
        public RegisterMultipleStudentsCommandValidator()
        {
            RuleFor(x => x.classId)
              .NotEmpty().WithMessage("ClassId is required.");
        }
    }
}
