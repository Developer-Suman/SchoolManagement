using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Visa.Application.Visa.Command.VisaApplication.AddVisaApplication
{
    public class AddVisaApplicationCommandValidator : AbstractValidator<AddVisaApplicationCommand>
    {
        public AddVisaApplicationCommandValidator()
        {
            RuleFor(x => x.countryId)
               .NotEmpty().WithMessage("CountryId is required.");
        }
    }
}
