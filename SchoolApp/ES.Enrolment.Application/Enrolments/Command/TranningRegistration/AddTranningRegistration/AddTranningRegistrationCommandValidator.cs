using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.TranningRegistration.AddTranningRegistration
{
    public class AddTranningRegistrationCommandValidator : AbstractValidator<AddTranningRegistrationCommand>
    {
        public AddTranningRegistrationCommandValidator()
        {
            RuleFor(x => x.applicantId)
            .NotEmpty()
            .WithMessage("ApplicantId is required.");
        }
    }
}
