using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ES.Communication.Application.Communication.Command.UpdateNotice
{
    public class UpdateNoticeCommandValidator  : AbstractValidator<UpdateNoticeCommand>
    {
        public UpdateNoticeCommandValidator()
        {
            RuleFor(x => x.id)
               .NotEmpty().WithMessage("Notice ID is required.")
               .Matches(@"\S").WithMessage("Notice ID must not be whitespace.");

        }
    }
}
