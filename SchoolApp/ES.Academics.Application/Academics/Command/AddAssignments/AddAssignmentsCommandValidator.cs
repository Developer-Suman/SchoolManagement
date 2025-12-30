using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddAssignments
{
    public class AddAssignmentsCommandValidator : AbstractValidator<AddAssignmentsCommand>
    {
        public AddAssignmentsCommandValidator()
        {
            RuleFor(x => x.title)
            .NotEmpty().WithMessage("Title is required.")
            .Matches(@"\S").WithMessage("Title must not be whitespace.")
            .MaximumLength(150).WithMessage("Title must not exceed 150 characters.");

            RuleFor(x => x.description)
                .NotEmpty().WithMessage("Description is required.")
                .Matches(@"\S").WithMessage("Description must not be whitespace.");

            RuleFor(x => x.dueDate)
                .NotEmpty().WithMessage("Due date is required.")
                .Must(date => date.Date >= DateTime.UtcNow.Date)
                .WithMessage("Due date cannot be in the past.");

            RuleFor(x => x.academicTeamId)
                .NotEmpty().WithMessage("Academic Team ID is required.")
                .Matches(@"\S").WithMessage("Academic Team ID must not be whitespace.");

            RuleFor(x => x.classId)
                .Must(x => string.IsNullOrWhiteSpace(x) == false)
                .When(x => x.classId != null)
                .WithMessage("Class ID must not be empty.");

            RuleFor(x => x.subjectId)
                .Must(x => string.IsNullOrWhiteSpace(x) == false)
                .When(x => x.subjectId != null)
                .WithMessage("Subject ID must not be empty.");
        }
    }
}
