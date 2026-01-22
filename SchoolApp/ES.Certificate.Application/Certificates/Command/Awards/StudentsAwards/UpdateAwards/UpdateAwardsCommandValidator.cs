using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.UpdateAwards
{
    public class UpdateAwardsCommandValidator : AbstractValidator<UpdateAwardsCommand>
    {
        public UpdateAwardsCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Award ID is required.")
                .Matches(@"\S").WithMessage("Award ID must not be whitespace.");
            RuleFor(x => x.schoolId)
                .NotEmpty().WithMessage("SchoolId is required.");

        }
    }
}
