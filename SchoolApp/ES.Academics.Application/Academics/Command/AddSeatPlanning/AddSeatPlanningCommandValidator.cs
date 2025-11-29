using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddSeatPlanning
{
    public class AddSeatPlanningCommandValidator : AbstractValidator<AddSeatPlanningCommand>
    {
        public AddSeatPlanningCommandValidator()
        {

            RuleFor(x => x.ExamSessionId)
             .NotEmpty()
             .WithMessage("ExamSession is required.")
             .MaximumLength(100)
             .WithMessage("Name cannot exceed 100 characters.");
        }
    }
   
   
}
