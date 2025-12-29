using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.UpdateStudentFee.RequestMapper
{
    public static class UpdateStudentFeeRequestMapper
    {
        public static UpdateStudentFeeCommand ToCommand(this UpdateStudentFeeRequest request, string Id)
        {
            return new UpdateStudentFeeCommand(
                Id,
                request.studentId,
                request.feeStructureId,
                request.discount,
                request.totalAmount,
                request.paidAmount
                 );
        }
    }
}
