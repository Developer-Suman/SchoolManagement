using AutoMapper;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Queries.FilterAttendances;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.Student.Queries.Attendance.AttendanceReport
{
    public class AttendanceReportQueryHandler : IRequestHandler<AttendanceReportQuery, Result<AttendanceReportResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IAttendanceServices _attendanceServices;

        public AttendanceReportQueryHandler(IAttendanceServices attendanceServices, IMapper mapper)
        {
            _mapper = mapper;
            _attendanceServices = attendanceServices;


        }
        public async Task<Result<AttendanceReportResponse>> Handle(AttendanceReportQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _attendanceServices.GetAttendanceReport(request.AttendanceReportDTOs);

                var attendanceReport = _mapper.Map<AttendanceReportResponse>(result.Data);

                return Result<AttendanceReportResponse>.Success(attendanceReport);
            }
            catch (Exception ex)
            {
                return Result<AttendanceReportResponse>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
