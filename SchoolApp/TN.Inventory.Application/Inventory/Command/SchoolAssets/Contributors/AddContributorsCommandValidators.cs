using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.Contributors
{
    public class AddContributorsCommandValidators : AbstractValidator<AddContributorsCommand>
    {
        public AddContributorsCommandValidators()
        {
            RuleFor(x => x.name)
                  .NotEmpty().WithMessage("Contributor name is required.");
        }
    }
}
