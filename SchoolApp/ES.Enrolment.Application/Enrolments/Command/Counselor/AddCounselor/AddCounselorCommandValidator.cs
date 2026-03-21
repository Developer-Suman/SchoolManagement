using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.Counselor.AddCounselor
{
    public class AddCounselorCommandValidator : AbstractValidator<AddCounselorCommand>
    {
        public AddCounselorCommandValidator()
        {
            RuleFor(x => x.fullName)
            .NotEmpty()
            .WithMessage("FullName is required.");
        }
    }
}
