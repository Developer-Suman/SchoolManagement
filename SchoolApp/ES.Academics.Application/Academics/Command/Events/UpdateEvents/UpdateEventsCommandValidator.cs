using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.Events.UpdateEvents
{
    public class UpdateEventsCommandValidator : FluentValidation.AbstractValidator<UpdateEventsCommand>
    {
        public UpdateEventsCommandValidator()
        {
            RuleFor(x => x.id)
               .NotEmpty().WithMessage("Event ID is required.")
               .Matches(@"\S").WithMessage("Event ID must not be whitespace.");
        }
    }
}
