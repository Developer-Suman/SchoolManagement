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

            RuleFor(x => x.countryId)
           .NotEmpty()
           .WithMessage("Country is required.");

            RuleFor(x => x.universityId)
          .NotEmpty()
          .WithMessage("University is required.");

            RuleFor(x => x.courseId)
         .NotEmpty()
         .WithMessage("Courses is required.");

        }
    }
}
