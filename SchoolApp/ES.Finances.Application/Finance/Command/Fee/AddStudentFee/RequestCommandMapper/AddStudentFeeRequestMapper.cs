using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.AddStudentFee.RequestCommandMapper
{
    public static class AddStudentFeeRequestMapper
    {
        public static AddStudentFeeCommand ToCommand(this AddStudentFeeRequest request)
        {
            return new AddStudentFeeCommand(
                request.studentId,
                request.feeStructureId,
                request.classId,
                request.discountPercentage
                );
        }
    }
}
