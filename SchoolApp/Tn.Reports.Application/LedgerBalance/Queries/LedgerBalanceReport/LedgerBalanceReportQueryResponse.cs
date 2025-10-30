﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.LedgerBalance.Queries.LedgerBalanceReport
{
    public record LedgerBalanceReportQueryResponse
    (
        string schoolId,
        string ledgerId,
        decimal balance
    );
}
