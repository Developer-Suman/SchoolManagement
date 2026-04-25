using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.FeeCategory.UpdateFeeCategory
{
    public class UpdateFeeCategoryCommandValidator : AbstractValidator<UpdateFeeCategoryCommand>
    {
        public UpdateFeeCategoryCommandValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Name is required.");
        }
    }
}
