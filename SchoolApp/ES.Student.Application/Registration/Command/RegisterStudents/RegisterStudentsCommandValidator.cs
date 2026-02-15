using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Registration.Command.RegisterStudents
{
    public class RegisterStudentsCommandValidator : AbstractValidator<RegisterStudentsCommand>
    {
        public RegisterStudentsCommandValidator()
        {
            RuleFor(x => x.studentId)
              .NotEmpty().WithMessage("StudentId is required.");
        }
    }
}
