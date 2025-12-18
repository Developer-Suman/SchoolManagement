using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItems
{
    public class AddSchoolItemsCommandValidators : AbstractValidator<AddSchoolItemsCommand>
    {
        public AddSchoolItemsCommandValidators()
        {
            RuleFor(x => x.name)
                  .NotEmpty().WithMessage("SchoolItems name is required.");
        }
    }
}
