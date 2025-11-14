using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Student.Command.UpdateParent
{
    public class UpdateParentCommandValidator : AbstractValidator<UpdateParentCommand>
    {
        public UpdateParentCommandValidator()
        {
            RuleFor(x => x.id)
            .NotEmpty().WithMessage(" id must be required.");
        }
    }
}
