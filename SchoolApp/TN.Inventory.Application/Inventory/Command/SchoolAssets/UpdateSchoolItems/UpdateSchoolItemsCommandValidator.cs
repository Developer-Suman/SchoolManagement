using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItems
{
    public class UpdateSchoolItemsCommandValidator:AbstractValidator<UpdateSchoolItemsCommand>
    {
        public UpdateSchoolItemsCommandValidator()
        {
            RuleFor(x => x.name)
               .NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.contributorId)
               .NotEmpty().WithMessage("Contributor ID is required.");
            RuleFor(x => x.itemCondition)
               .IsInEnum().WithMessage("Invalid item condition.");
            RuleFor(x => x.receivedDate)
               .LessThanOrEqualTo(DateTime.Now).WithMessage("Received date cannot be in the future.");
            RuleFor(x => x.estimatedValue)
               .GreaterThanOrEqualTo(0).When(x => x.estimatedValue.HasValue).WithMessage("Estimated value must be non-negative.");
            RuleFor(x => x.quantity)
               .GreaterThan(0).When(x => x.quantity.HasValue).WithMessage("Quantity must be greater than zero.");
            RuleFor(x => x.unitType)
               .IsInEnum().When(x => x.unitType.HasValue).WithMessage("Invalid unit type.");
        }

    }
}
