using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;

namespace TN.Reports.Application.SchoolReports.CoCurricularActivityReport
{
    public record CoCurricularActivitiesReportDTOs
    (
        string? startDate,
        string? endDate,
        string? activityName,
        ActivityCategory? activityCategory
        );
}
