using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Student.Command.AddAttendances
{
    public class AddAttendenceCommandValidator : AbstractValidator<AddAttendenceCommand>
    {
        public AddAttendenceCommandValidator()
        {
            RuleFor(x => x.AcademicTeamId)
              .NotEmpty().WithMessage("fullName is required.");
        }
    }
}
