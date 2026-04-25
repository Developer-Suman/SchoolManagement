using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.UpdateStudentFee
{
    public class UpdateStudentFeeCommandValidator : AbstractValidator<UpdateStudentFeeCommand>
    {
        public UpdateStudentFeeCommandValidator()
        {
            RuleFor(x => x.id)
               .NotEmpty().WithMessage("FeeType ID is required.")
               .Matches(@"\S").WithMessage("FeeType ID must not be whitespace.");
            RuleFor(x => x.studentId)
                .NotEmpty().WithMessage("student Id is required.");

        }
    }
}
