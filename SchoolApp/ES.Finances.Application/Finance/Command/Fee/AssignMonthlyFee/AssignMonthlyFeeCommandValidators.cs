using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.AssignMonthlyFee
{
    public class AssignMonthlyFeeCommandValidators : AbstractValidator<AssignMonthlyFeeCommand>
    {
        public AssignMonthlyFeeCommandValidators()
        {
            RuleFor(x => x.classId)
            .NotEmpty()
            .WithMessage("classId is required.");

        }
    }
}
