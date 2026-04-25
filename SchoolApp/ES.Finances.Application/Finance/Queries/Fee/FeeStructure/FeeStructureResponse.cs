using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Finance;

namespace ES.Finances.Application.Finance.Queries.Fee.FeeStructure
{
    public record FeeStructureResponse
    (
         string id,
            string classId,
            string fyId,
            string? ledgerId,
            //NameOfMonths? nameOfMonths,
            string? feeCategoryId,
            //List<FeeStructureDetails> feeStructureDetails,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
        );
}
