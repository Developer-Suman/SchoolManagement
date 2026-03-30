using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.SchoolReports.AttendanceReport;
using TN.Reports.Application.SchoolReports.CoCurricularActivityReport;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.ServiceInterface
{
    public interface ISchoolReportServices
    {
        Task<Result<AttendanceReportResponse>> GetAttendanceReport(AttendanceReportDTOs attendanceReportDTOs);
        Task<Result<PagedResult<CoCurricularActivitiesReportResponse>>> GetCocurricularActivitiesReports(CoCurricularActivitiesReportDTOs coCurricularActivitiesReportDTOs, PaginationRequest paginationRequest);
    }
}
