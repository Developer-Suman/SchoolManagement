using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Invoice.AddInvoice
{
    public class AddInvoiceCommandValidator : AbstractValidator<AddInvoiceCommand>
    {
        public AddInvoiceCommandValidator() 
        {
            RuleFor(x => x.applicantId)
                .NotEmpty().WithMessage("ApplicantId is required.");
        }
    }
}
