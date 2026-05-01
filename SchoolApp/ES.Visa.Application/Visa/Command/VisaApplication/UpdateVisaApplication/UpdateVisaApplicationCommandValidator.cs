using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Visa.Application.Visa.Command.VisaApplication.UpdateVisaApplication
{
    public class UpdateVisaApplicationCommandValidator : AbstractValidator<UpdateVisaApplicationCommand>
    {
        public UpdateVisaApplicationCommandValidator()
        {
            RuleFor(x => x.countryId)
               .NotEmpty().WithMessage("CountryId is required.");
        }
    }
}
