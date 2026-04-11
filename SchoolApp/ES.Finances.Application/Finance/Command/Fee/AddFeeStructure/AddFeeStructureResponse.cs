using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Finances.Application.Finance.Command.Fee.AddFeeStructure
{
    public record AddFeeStructureResponse
    (
        string id="",
            decimal amount=0,
            string classId="",
            string fyId = "",
            string feeTypeId = "",
            bool isActive=true,
            string schoolId = "",
            string createdBy = "",
            DateTime createdAt=default,
            string modifiedBy = "",
            DateTime modifiedAt= default,
                  List<AddFeeStructureDTOs> feeStructureDTOs = default
        );
}
