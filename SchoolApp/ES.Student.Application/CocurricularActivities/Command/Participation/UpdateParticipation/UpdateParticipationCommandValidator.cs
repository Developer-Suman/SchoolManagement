using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.CocurricularActivities.Command.Participation.UpdateParticipation
{
    public class UpdateParticipationCommandValidator : AbstractValidator<UpdateParticipationCommand>
    {
        public UpdateParticipationCommandValidator()
        {
            RuleFor(x => x.studentId)
            .NotEmpty().WithMessage(" studentId must be required.");

        }
    }
}
