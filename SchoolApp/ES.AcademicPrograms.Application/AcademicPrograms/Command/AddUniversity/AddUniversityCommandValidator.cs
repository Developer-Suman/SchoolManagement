using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddUniversity
{
    public class AddUniversityCommandValidator : AbstractValidator<AddUniversityCommand>
    {
        public AddUniversityCommandValidator()
        {
            RuleFor(x => x.name)
            .NotEmpty()
            .WithMessage("Name is required.");
        }
    }
}
