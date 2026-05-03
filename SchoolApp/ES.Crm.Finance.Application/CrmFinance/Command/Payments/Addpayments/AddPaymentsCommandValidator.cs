using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Payments.Addpayments
{
    public class AddPaymentsCommandValidator : AbstractValidator<AddPaymentsCommand>
    {
        public AddPaymentsCommandValidator()
        {
            RuleFor(x => x.invoiceId).NotEmpty().WithMessage("Invoice ID is required.");
            RuleFor(x => x.amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
            RuleFor(x => x.paymentDate).NotEmpty().WithMessage("Payment date is required.");
            RuleFor(x => x.paymentMethod).IsInEnum().WithMessage("Invalid payment method.");
            RuleFor(x => x.referenceNumber).NotEmpty().WithMessage("Reference number is required.");
            RuleFor(x => x.paymentStatus).IsInEnum().WithMessage("Invalid payment status.");
        }
    }
}
