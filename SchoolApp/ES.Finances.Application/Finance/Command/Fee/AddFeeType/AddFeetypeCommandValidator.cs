using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.AddFeeType
{
    public class AddFeetypeCommandValidator : AbstractValidator<AddFeeTypeCommand>
    {
        public AddFeetypeCommandValidator()
        {
            RuleFor(x => x.name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters.");
        }
    }
}
