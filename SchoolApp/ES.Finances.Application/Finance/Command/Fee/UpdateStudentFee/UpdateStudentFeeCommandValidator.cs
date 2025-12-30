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
            RuleFor(x => x.feeStructureId)
                .NotEmpty().WithMessage("Fee Structure Id is required.");
            RuleFor(x => x.discount)
                .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.");
            RuleFor(x => x.totalAmount)
                .GreaterThan(0).WithMessage("Total Amount must be greater than zero.");
            RuleFor(x => x.paidAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Paid Amount cannot be negative.")
                .LessThanOrEqualTo(x => x.totalAmount).WithMessage("Paid Amount cannot be greater than Total Amount.");

        }
    }
}
