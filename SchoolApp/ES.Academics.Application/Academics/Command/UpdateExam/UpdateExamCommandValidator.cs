using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.UpdateExam
{
    internal class UpdateExamCommandValidator : AbstractValidator<UpdateExamCommand>
    {
        public UpdateExamCommandValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Exam ID is required.")
                .Matches(@"\S").WithMessage("Exam ID must not be whitespace.");
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Exam name is required.");
        }
    }
    
}
