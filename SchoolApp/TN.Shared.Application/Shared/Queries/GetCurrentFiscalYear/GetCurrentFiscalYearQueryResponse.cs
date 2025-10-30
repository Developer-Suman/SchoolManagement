using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Queries.GetCurrentFiscalYear
{
    public record GetCurrentFiscalYearQueryResponse
    (
         string? currentFiscalYearId,
         string schoolId,
         string startDate,
         string? fyName
    );
}
