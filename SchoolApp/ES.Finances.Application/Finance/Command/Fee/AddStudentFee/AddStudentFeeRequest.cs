using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Finances.Application.Finance.Command.Fee.AddStudentFee
{
    public record AddStudentFeeRequest
    (
        string studentId,
            string feeStructureId,
            string classId,
            decimal discountPercentage,
            List<StudentFeeDetailsDTOs?> StudentFeeDetailsDTOs
        );
}
