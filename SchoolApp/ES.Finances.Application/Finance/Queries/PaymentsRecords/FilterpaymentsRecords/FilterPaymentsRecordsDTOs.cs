using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Queries.PaymentsRecords.FilterpaymentsRecords
{
    public record FilterPaymentsRecordsDTOs
    (
        string? classid,
        string? studentId,
        string? startDate,
        string? endDate
        );
}
