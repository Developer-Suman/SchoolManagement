using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Communication.Application.Communication.Command.AddNotice
{
    public class AddNoticeCommandValidator : AbstractValidator<AddNoticeCommand>
    {
        public AddNoticeCommandValidator() 
        {
            RuleFor(x => x.title)
              .NotEmpty().WithMessage("Title is required.");
        }
    }
}
