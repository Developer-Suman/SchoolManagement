using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.PaymentRecords.AddpaymentsRecords
{
    public class AddPaymentsRecordsCommandValidators : AbstractValidator<AddPaymentsRecordsCommand>
    {
        public AddPaymentsRecordsCommandValidators()
        {
            RuleFor(x => x.studentfeeId)
            .NotEmpty()
            .WithMessage("StudentFeeid is required.");
        }
    }
}
