using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.Staff.Command.AddAcademicTeam
{
    public class AddAcademicTeamCommandValidator : AbstractValidator<AddAcademicTeamCommand>
    {
        public AddAcademicTeamCommandValidator()
        {
            RuleFor(x => x.address)
                         .NotEmpty()
                         .WithMessage("address is required.");


            RuleFor(x => x.email)
                .NotEmpty()
                .WithMessage("Email is required.");
        }
    }
}
