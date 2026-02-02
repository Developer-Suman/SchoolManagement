using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.ConvertApplicant
{
    public class ConvertApplicantCommandValidators: AbstractValidator<ConvertApplicantCommand>
    {
        public ConvertApplicantCommandValidators() 
        {
            RuleFor(x => x.passportNo)
            .NotEmpty()
            .WithMessage("passports No is required.");
        }
    }
}
