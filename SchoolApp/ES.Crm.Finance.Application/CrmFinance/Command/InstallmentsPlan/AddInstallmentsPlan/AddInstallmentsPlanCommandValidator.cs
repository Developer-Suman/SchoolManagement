using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan
{
    public class AddInstallmentsPlanCommandValidator : AbstractValidator<AddInstallmentsPlanCommand>
    {
        public AddInstallmentsPlanCommandValidator()
        {
            RuleFor(x => x.invoiceId)
               .NotEmpty().WithMessage("InvoiceId is required.");

            RuleFor(x => x.numberOfInstallments)
               .GreaterThan(0).WithMessage("Number of installments must be greater than zero.");

        }
    }
}
