using ES.Finances.Application.Finance.Command.Fee.AddStudentFee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Command.Fee.UpdateStudentFee
{
    public record UpdateStudentFeeCommand
    (
        string id,
        string studentId,
            string feeStructureId,
            string classId,
            decimal discountPercentage,
            List<UpdateStudentFeeDetailsDTOs?> StudentFeeDetailsDTOs
        ) : IRequest<Result<UpdateStudentFeeResponse>>;
}
