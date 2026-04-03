using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Finances.Application.Finance.Command.Fee.AddFeeStructure
{
    public record AddFeeStructureDTOs
    (
        string feeTypeId,
            string feeStructureId,
            decimal amount,
            int times,
            decimal totalAmount,
            FeePaidType feePaidType
        );
}
