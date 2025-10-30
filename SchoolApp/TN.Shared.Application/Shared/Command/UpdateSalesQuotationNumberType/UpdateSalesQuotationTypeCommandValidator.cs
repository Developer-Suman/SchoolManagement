using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Shared.Application.Shared.Command.UpdateSalesQuotationNumberType
{
    public  class UpdateSalesQuotationTypeCommandValidator:AbstractValidator<UpdateSalesQuotationTypeCommand>
    {
        public UpdateSalesQuotationTypeCommandValidator()
        {
            RuleFor(x => x.schoolId)
             .NotEmpty().WithMessage("SchoolId is required.");
        }
    }
}
