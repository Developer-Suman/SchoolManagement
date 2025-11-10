using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.UpdateSubject
{
    public class UpdateSubjectCommandValidator : AbstractValidator<UpdateSubjectCommand>
    {
        public UpdateSubjectCommandValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Exam ID is required.")
                .Matches(@"\S").WithMessage("Exam ID must not be whitespace.");
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Exam name is required.");
        }
    }
}
