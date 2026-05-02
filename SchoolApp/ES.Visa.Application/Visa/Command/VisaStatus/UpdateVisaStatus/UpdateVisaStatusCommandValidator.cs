using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ES.Visa.Application.Visa.Command.VisaStatus.UpdateVisaStatus
{
    public class UpdateVisaStatusCommandValidator : AbstractValidator<UpdateVisaStatusCommand>
    {
        public UpdateVisaStatusCommandValidator()
        {
            RuleFor(x => x.VisaStatusType)
               .NotEmpty().WithMessage("VisaStatusType is required.");
        }
    }
}
