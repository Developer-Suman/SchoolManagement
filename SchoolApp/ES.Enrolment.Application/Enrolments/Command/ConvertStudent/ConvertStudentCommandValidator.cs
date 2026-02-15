using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.ConvertStudent
{
    public class ConvertStudentCommandValidator : AbstractValidator<ConvertStudentCommand>
    {
        public ConvertStudentCommandValidator()
        {
            RuleFor(x => x.universityName)
            .NotEmpty()
            .WithMessage("universityName is required.");
        }
    }
}
