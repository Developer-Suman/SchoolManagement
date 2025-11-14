using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddExamResult
{
    public class AddExamResultCommandValidator : AbstractValidator<AddExamResultCommand>
    {
        public AddExamResultCommandValidator() {
            RuleFor(x => x.studentId);
             //.GreaterThanOrEqualTo(0)
             //.WithMessage("Marks Obtained must be greater than or equal to 0.");

               
        }
    }
}
