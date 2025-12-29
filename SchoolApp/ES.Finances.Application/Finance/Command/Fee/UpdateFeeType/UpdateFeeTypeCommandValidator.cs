using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.UpdateFeeType
{
    public class UpdateFeeTypeCommandValidator : AbstractValidator<UpdateFeeTypeCommand>
    {
        public UpdateFeeTypeCommandValidator()
        {
            RuleFor(x => x.id)
               .NotEmpty().WithMessage("FeeType ID is required.")
               .Matches(@"\S").WithMessage("FeeType ID must not be whitespace.");
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("FeeType name is required.");
            RuleFor(x => x.description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
            RuleFor(x => x.nameOfMonths)
                .IsInEnum().WithMessage("Invalid value for NameOfMonths.");
        }
    }
}
