using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Payments.UpdatePayments
{
    public class UpdatePaymentsCommandValidator : AbstractValidator<UpdatePaymentsCommand>
    {
        public UpdatePaymentsCommandValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}
