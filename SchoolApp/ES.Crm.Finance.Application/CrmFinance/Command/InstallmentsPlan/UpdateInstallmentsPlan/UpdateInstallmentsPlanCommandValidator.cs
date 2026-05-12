using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.UpdateInstallmentsPlan
{
    public class UpdateInstallmentsPlanCommandValidator : AbstractValidator<UpdateInstallmentsPlanCommand>
    {
        public UpdateInstallmentsPlanCommandValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Id is required.");
            RuleFor(x => x.invoiceId)
                .NotEmpty().WithMessage("Invoice Id is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        }
    }
}
