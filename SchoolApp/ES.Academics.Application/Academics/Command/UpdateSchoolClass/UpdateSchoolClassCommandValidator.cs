using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.UpdateSchoolClass
{
    public class UpdateSchoolClassCommandValidator : AbstractValidator<UpdateSchoolClassCommand>
    {
        public UpdateSchoolClassCommandValidator()
        {
    
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Class ID is required.")
                .Matches(@"\S").WithMessage("Class ID must not be whitespace.");


            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Class name is required.");



        }
    
    }
}
