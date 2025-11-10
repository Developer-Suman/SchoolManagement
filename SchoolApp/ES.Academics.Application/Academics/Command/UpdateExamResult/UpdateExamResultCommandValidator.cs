using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.UpdateExamResult
{
    public class UpdateExamResultCommandValidator : AbstractValidator<UpdateExamResultCommand>
    {
        public UpdateExamResultCommandValidator()
        {
            RuleFor(x => x.id)
               .NotEmpty().WithMessage("ExamResult ID is required.")
               .Matches(@"\S").WithMessage("Exam ID must not be whitespace.");
        }
    }
}
