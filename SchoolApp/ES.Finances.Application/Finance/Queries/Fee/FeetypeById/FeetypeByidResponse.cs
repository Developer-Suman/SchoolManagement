using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Finances.Application.Finance.Queries.Fee.FeetypeById
{
    public record FeetypeByidResponse
   (
        string id,
            string name,
            string? description,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            string? fyId,
            NameOfMonths? nameOfMonths
        );
}
