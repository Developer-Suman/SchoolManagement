using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.UpdateFeeStructure
{
    public class UpdateFeeStructureCommandValidator : AbstractValidator<UpdateFeeStructureCommand>
    {
        public UpdateFeeStructureCommandValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("amount is required.")
                .Matches(@"\S").WithMessage("amount must not be whitespace.");
        }
    }
}
