using ES.Student.Application.Student.Command.AddAttendances;
using ES.Student.Application.Student.Queries.Attendance.AttendanceReport;
using ES.Student.Application.Student.Queries.FilterAttendances;
using ES.Student.Application.Student.Queries.FilterStudents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.ServiceInterface
{
    public interface IAttendanceServices
    {
        Task<Result<IEnumerable<AddAttendanceResponse>>> MarkBulkAsync(AddAttendenceCommand request);
        Task<Result<PagedResult<FilterAttendanceResponse>>> GetFilterStudentAttendance(PaginationRequest paginationRequest, FilterAttendanceDTOs filterAttendanceDTOs);
        Task<Result<AttendanceReportResponse>> GetAttendanceReport(AttendanceReportDTOs attendanceReportDTOs);
    }
}
