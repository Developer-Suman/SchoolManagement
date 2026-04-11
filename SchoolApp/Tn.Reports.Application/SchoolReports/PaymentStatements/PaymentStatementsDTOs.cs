using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.SchoolReports.PaymentStatements
{
    public record PaymentStatementsDTOs
    (
          string? startDate,
        string? endDate,
        string? studentId
        );
}
