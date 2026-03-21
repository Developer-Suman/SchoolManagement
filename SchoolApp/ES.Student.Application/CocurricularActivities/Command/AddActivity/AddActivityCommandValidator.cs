using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.CocurricularActivities.Command.AddActivity
{
    public class AddActivityCommandValidator : AbstractValidator<AddActivityCommand>
    {
        public AddActivityCommandValidator()
        {
            RuleFor(x => x.name)
             .NotEmpty().WithMessage("Name is required.");
        }
    }
}
