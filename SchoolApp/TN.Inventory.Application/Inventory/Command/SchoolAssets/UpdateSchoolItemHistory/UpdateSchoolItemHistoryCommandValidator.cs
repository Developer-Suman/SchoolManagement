using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItemHistory
{
    public class UpdateSchoolItemHistoryCommandValidator:AbstractValidator<UpdateSchoolItemHistoryCommand>
    {
        public UpdateSchoolItemHistoryCommandValidator()
        {
            RuleFor(x => x.currentStatus)
               .NotEmpty().WithMessage("Current Status is required.");
        }
    }
}
