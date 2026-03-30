using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;

namespace TN.Reports.Application.SchoolReports.CoCurricularActivityReport
{
    public record CoCurricularActivitiesReportResponse
    (
        string EventsId,
        DateTime ActivityDate,
        List<ActivityDetailDto> Activities
        //string? totalBudget
        );

    public record ActivityDetailDto
    (
        string ActivityName,
        string? ActivityDescription,
        ActivityCategory ActivityCategory,
        int Participants,
        List<string> ClassIds
    );
}
