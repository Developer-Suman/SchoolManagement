using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.AddInquiry
{
    public class AddInquiryCommandValidators : AbstractValidator<AddInquiryCommand>
    {
        public AddInquiryCommandValidators()
        {
            RuleFor(x => x.fullName)
            .NotEmpty()
            .WithMessage("fullName is required.");
        }
    }
}
