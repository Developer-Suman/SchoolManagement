using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.FollowUp.AddFollowUp
{
    public class AddFollowUpCommandValidator : AbstractValidator<AddFollowUpCommand>
    {
        public AddFollowUpCommandValidator()
        {
            RuleFor(x => x.leadId)
            .NotEmpty()
            .WithMessage("LeadId is required.");
        }
    }
}
