using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.Awards.AddAwards
{
    public class AddAwardsCommandValidator : AbstractValidator<AddAwardsCommand>
    {
        public AddAwardsCommandValidator()
        {
            RuleFor(x => x.studentId)
                .NotEmpty().WithMessage("Student ID is required.");
            RuleFor(x => x.awardedAt)
                .NotEmpty().WithMessage("Awarded date is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Awarded date cannot be in the future.");
            RuleFor(x => x.awardedBy)
                .NotEmpty().WithMessage("Awarded by is required.");
            RuleFor(x => x.awardDescriptions)
                .NotEmpty().WithMessage("Award descriptions are required.");
        }
    }
}
