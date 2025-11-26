using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.Staff.Command.UnAssignedClassToAcademicTeam
{
    public class UnAssignClassCommandValidator : AbstractValidator<UnAssignClassCommand>
    {
        public UnAssignClassCommandValidator()
        {
            RuleFor(x => x.ClassesId)
                         .NotEmpty()
                         .WithMessage("ClassId is required.");


            RuleFor(x => x.AcademicTeamId)
                .NotEmpty()
                .WithMessage("AcademicTeamId is required.");
        }
    }
}
