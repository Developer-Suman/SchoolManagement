using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.UpdateFeeStructure
{
    public record UpdateFeeStructureResponse
    (
        string id="",
        string classId="",
            string feeCategoryId="",
            List<AddFeeStructureDTOs> feeStructureDTOs = default
        );
}
