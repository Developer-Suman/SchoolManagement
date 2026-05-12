using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;

namespace ES.Finances.Application.Finance.Queries.Fee.DueSlip
{
    public record DueSlipResponse
    (
        string? studentName="",
        string? address="",
        string? schoolId="",
        string? classId="",
        string? className="",
        decimal discount = 0,
            decimal totalAmount = 0,
            decimal paidAmount = 0,
        List<FeeStructureForDueSlipDTOs> feeStructures=default
    );
}
