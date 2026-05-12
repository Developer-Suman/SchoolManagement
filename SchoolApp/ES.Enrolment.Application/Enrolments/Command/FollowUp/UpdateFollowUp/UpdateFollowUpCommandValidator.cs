using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.FollowUp.UpdateFollowUp
{
    public class UpdateFollowUpCommandValidator : AbstractValidator<UpdateFollowUpCommand>
    {
        public UpdateFollowUpCommandValidator()
        {
            RuleFor(x => x.appointmentId)
            .NotEmpty()
            .WithMessage("appointmentId is required.");
        }
    }
}
