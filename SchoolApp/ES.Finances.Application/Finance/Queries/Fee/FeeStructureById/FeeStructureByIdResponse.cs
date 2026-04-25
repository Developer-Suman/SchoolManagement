using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Finances.Application.Finance.Queries.Fee.FeeStructureById
{
    public record FeeStructureByIdResponse
    (
        string id,
            string classId,
            string feeCategoryId,
            string feeCategoryName,
            string fyId,
            List<AddFeeStructureDTOs> feeStructureDTOs,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt

        );
}
