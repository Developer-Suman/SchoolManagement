using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TN.Setup.Application.Setup.Command.AddSchool;

namespace TN.Setup.Application.Setup.Command.AddSchool
{
  public class AddSchoolCommandValidator: AbstractValidator<AddSchoolCommand>
    {

        public AddSchoolCommandValidator() 
        {
            RuleFor(x => x.name)
                    .NotEmpty()
                    .WithMessage("Institution name is required.");


            RuleFor(x => x.address)
                .NotEmpty()
                .WithMessage("Address is required.");
                
        }
    }
}
