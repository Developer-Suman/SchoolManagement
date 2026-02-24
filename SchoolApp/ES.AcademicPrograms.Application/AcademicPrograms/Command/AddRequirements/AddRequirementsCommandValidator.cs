using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddRequirements
{
    public class AddRequirementsCommandValidator : AbstractValidator<AddRequirementsCommand>
    {
        public AddRequirementsCommandValidator()
        {
            RuleFor(x => x.descriptions)
            .NotEmpty()
            .WithMessage("Descriptions is required.");

        }
    }
}
