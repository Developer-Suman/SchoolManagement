using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.AddStudentFee
{
    public class AddStudentFeeCommandValidator : AbstractValidator<AddStudentFeeCommand>
    {
        public AddStudentFeeCommandValidator()
        {
            RuleFor(x => x.studentId)
            .NotEmpty()
            .WithMessage("Studentid is required.");
        }
    }
    
}
