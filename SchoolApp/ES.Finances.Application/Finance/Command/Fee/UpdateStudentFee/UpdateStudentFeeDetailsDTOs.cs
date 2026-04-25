using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Finances.Application.Finance.Command.Fee.UpdateStudentFee
{
    public record UpdateStudentFeeDetailsDTOs
    (
        string? id,
        string feeTypeId,
            decimal? discountAmount,
            decimal amount,
            int times,
            decimal totalAmount,
            FeePaidType feePaidType
        );
}
