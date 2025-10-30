using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Shared.Application.Shared.Command.UpdateTaxStatusInPurchase
{
    public  class UpdateTaxStatusInPurchaseCommandValidator:AbstractValidator<UpdateTaxStatusInPurchaseCommand>
    {
        public UpdateTaxStatusInPurchaseCommandValidator()
        {
            RuleFor(x => x.schoolId).NotEmpty().WithMessage("SchoolId is required");

        }
    }
}
