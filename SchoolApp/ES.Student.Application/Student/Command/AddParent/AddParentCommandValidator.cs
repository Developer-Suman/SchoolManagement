using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ES.Student.Application.Student.Command.AddParent
{
    public  class AddParentCommandValidator:AbstractValidator<AddParentCommand>
    {
        public AddParentCommandValidator()
        {
            RuleFor(x => x.fullName)
              .NotEmpty().WithMessage("fullName is required.");
        }
    }
}
