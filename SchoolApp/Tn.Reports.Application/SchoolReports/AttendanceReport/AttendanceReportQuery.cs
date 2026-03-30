using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Reports.Application.SchoolReports.AttendanceReport
{
    public record AttendanceReportQuery
    (
        AttendanceReportDTOs AttendanceReportDTOs
        ): IRequest<Result<AttendanceReportResponse>>;
}
