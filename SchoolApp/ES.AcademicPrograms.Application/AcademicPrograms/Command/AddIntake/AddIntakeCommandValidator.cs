using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddIntake
{
    public class AddIntakeCommandValidator : AbstractValidator<AddIntakeCommand>
    {
        public AddIntakeCommandValidator()
        {
            RuleFor(x => x.month)
            .NotEmpty()
            .WithMessage("Months is required.");
        }
    }
}
