using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Visa.Application.Visa.Command.VisaStatus.AddVisaStatus
{
    public class AddVisaStatusCommandValidator : AbstractValidator<AddVisaStatusCommand>
    {
        public AddVisaStatusCommandValidator()
        {
            RuleFor(x => x.name)
               .NotEmpty().WithMessage("Name is required.");

        }
    }
}
