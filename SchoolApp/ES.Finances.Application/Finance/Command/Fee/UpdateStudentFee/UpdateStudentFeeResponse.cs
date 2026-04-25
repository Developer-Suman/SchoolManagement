using ES.Finances.Application.Finance.Command.Fee.AddStudentFee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.UpdateStudentFee
{
    public record UpdateStudentFeeResponse
    (
        string id,
        string studentId,
            string feeStructureId,
            string classId,
            decimal discountPercentage
        );
}
