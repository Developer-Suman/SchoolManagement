using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Finances.Application.Finance.Queries.Fee.DueSlip
{
    public record FeeStructureForDueSlipDTOs
    (
        string feeTypeId,
        string feeTypeName,
    decimal amount,
    decimal? discountAmount,
    int times,
    decimal totalAmount,
    FeePaidType feePaidType
        );
}
