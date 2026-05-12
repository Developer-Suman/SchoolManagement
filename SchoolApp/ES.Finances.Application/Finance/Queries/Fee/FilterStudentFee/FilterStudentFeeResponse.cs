using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Queries.Fee.FilterStudentFee
{
    public record FilterStudentFeeResponse
    (
        string id="",
            string studentId="",
            string feeStructureId="",
            decimal totalAmount = 0,
            decimal paidAmount=0,
            decimal dueAmount=0,
            string classId="",
            string? schoolId="",
            string? receiptNumber = "",
            List<FeeStructureDTOs> feeStructureDTOs = default
        );
}
