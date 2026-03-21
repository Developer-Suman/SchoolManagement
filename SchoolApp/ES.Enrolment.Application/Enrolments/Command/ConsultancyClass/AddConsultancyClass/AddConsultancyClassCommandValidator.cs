using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.ConsultancyClass
{
    public class AddConsultancyClassCommandValidator : AbstractValidator<AddConsultancyClassCommand>
    {
        public AddConsultancyClassCommandValidator()
        {
            RuleFor(x => x.name)
            .NotEmpty()
            .WithMessage("Class Name is required.");
        }
    }
}
