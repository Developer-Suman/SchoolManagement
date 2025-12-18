using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItemHistory
{
    public class AddSchoolItemsHistoryCommandValidator : AbstractValidator<AddSchoolItemHistoryCommand>
    {
        public AddSchoolItemsHistoryCommandValidator()
        {
            RuleFor(x => x.schoolItemId)
                  .NotEmpty().WithMessage("SchoolItems is required.");
        }
    }
}
