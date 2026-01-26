using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.Events.AddEvents
{
    public class AddEventsCommandValidator : AbstractValidator<AddEventsCommand>
    {
        public AddEventsCommandValidator()
        {
            RuleFor(x => x.title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");
            RuleFor(x => x.descriptions)
                .MaximumLength(1000).WithMessage("Descriptions cannot exceed 1000 characters.");
            RuleFor(x => x.eventsType)
                .NotEmpty().WithMessage("Events type is required.")
                .MaximumLength(100).WithMessage("Events type cannot exceed 100 characters.");
            //RuleFor(x => x.eventsDate)
            //    .NotEmpty().WithMessage("Events date is required.")
            //    .LessThanOrEqualTo(DateTime.Now).WithMessage("Events date cannot be in the future.");
            RuleFor(x => x.participants)
                .NotEmpty().WithMessage("Participants are required.")
            //    .MaximumLength(500).WithMessage("Participants cannot exceed 500 characters.");
            //RuleFor(x => x.eventTime)
            //    .NotEmpty().WithMessage("Event time is required.");
            //RuleFor(x => x.venue)
                .NotEmpty().WithMessage("Venue is required.")
                .MaximumLength(300).WithMessage("Venue cannot exceed 300 characters.");
            RuleFor(x => x.chiefGuest)
                .MaximumLength(200).WithMessage("Chief guest cannot exceed 200 characters.");
            RuleFor(x => x.organizer)
                .MaximumLength(200).WithMessage("Organizer cannot exceed 200 characters.");
            RuleFor(x => x.mentor)
                .MaximumLength(200).WithMessage("Mentor cannot exceed 200 characters.");
        }
    }
}
