using ES.Finances.Application.Finance.Command.Fee.AddStudentFee;
using ES.Finances.Application.Finance.Command.Fee.UpdateStudentFee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Queries.Fee.StudentFeeById
{
    public record StudentFeeByIdResponse
    (
        string id,
            string studentId,
            string feeStructureId,
            string classId,
            decimal discountPercentage,
            List<UpdateStudentFeeDetailsDTOs?> StudentFeeDetailsDTOs
        );
}
