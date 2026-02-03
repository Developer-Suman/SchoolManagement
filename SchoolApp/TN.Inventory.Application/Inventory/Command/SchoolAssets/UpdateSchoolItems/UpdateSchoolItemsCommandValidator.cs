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
        }

    }
}
