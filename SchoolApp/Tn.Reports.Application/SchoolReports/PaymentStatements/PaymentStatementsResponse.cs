using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.SchoolReports.PaymentStatements
{
    public record PaymentStatementsResponse
    (
        string ? schoolId,
        string ? studentId,    
        DateTime ? date,
        string ? receiptNumber,
        decimal ? debitAmount,
        decimal ? creditAmount,
        decimal? adjustment,
        decimal? balance,
        string? remarks
        );
}
