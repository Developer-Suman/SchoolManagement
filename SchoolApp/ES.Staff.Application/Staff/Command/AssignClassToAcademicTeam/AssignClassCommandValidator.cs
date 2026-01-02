using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.Staff.Command.AssignClassToAcademicTeam
{
    public class AssignClassCommandValidator : AbstractValidator<AssignClassCommand>
    {
        public AssignClassCommandValidator()
        {



            RuleFor(x => x.AcademicTeamId)
                .NotEmpty()
                .WithMessage("AcademicTeamId is required.");
        }
    }
}
