using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Queries.Fee.FilterStudentFee
{
    public record FilterStudentFeeDTOs
    (
          string? studentId,
        string? startDate,
        string? endDate
        );
}
