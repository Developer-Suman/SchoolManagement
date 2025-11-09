using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddSchoolClass
{
    public class AddSchoolClassCommandValidator : AbstractValidator<AddSchoolClassCommand>
    {
        public AddSchoolClassCommandValidator()
        {
            RuleFor(x => x.name)
             .NotEmpty()
             .WithMessage("Name is required.")
             .MaximumLength(100)
             .WithMessage("Name cannot exceed 100 characters.");
        }
    }
}
