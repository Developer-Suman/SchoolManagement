using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Command.Fee.AddStudentFee
{
    public record AddStudentFeeCommand
    (
        string studentId,
            string feeStructureId,
            string classId,
            decimal discountPercentage
        ) : IRequest<Result<AddStudentFeeResponse>>;
}
