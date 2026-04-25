using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.CocurricularActivities.Command.Participation.Addparticipation
{
    public class AddParticipationCommandValidator : AbstractValidator<AddParticipationCommand>
    {
        public AddParticipationCommandValidator()
        {
            RuleFor(x => x.studentId)
              .NotEmpty().WithMessage("StudentId is required.");
        }
    }
}
