using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Setup.Application.Setup.Command.UpdateSchool
{
   public class UpdateSchoolCommandValidator :AbstractValidator<UpdateSchoolCommand>
    {
        public UpdateSchoolCommandValidator() 
        {
            RuleFor(x => x.name)
                        .NotEmpty()
                        .WithMessage("School name is required.");


            RuleFor(x => x.address)
                .NotEmpty()
                .WithMessage("Address is required.");

        }
    }
}
