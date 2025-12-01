using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddExamSession
{
    public class AddExamSessionCommandValidator : AbstractValidator<AddExamSessionCommand>
    {
        public AddExamSessionCommandValidator()
        {
            RuleFor(x => x.name);
        }
    }
}
