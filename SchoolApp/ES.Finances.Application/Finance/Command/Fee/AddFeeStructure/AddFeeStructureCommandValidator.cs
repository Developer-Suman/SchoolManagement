using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.AddFeeStructure
{
    public class AddFeeStructureCommandValidator : AbstractValidator<AddFeeStructureCommand>
    {
        public AddFeeStructureCommandValidator()
        {
            RuleFor(x => x.classId)
            .NotEmpty()
            .WithMessage("Classid is required.");
  
        }
    }
}
