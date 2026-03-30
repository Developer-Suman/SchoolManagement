using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Reports.Application.SchoolReports.AttendanceReport
{
    public class AttendanceReportQueryHandler : IRequestHandler<AttendanceReportQuery, Result<AttendanceReportResponse>>
    {

        private readonly IMapper _mapper;
        private readonly ISchoolReportServices _schoolReportServices;

        public AttendanceReportQueryHandler(ISchoolReportServices schoolReportServices, IMapper mapper)
        {
            _mapper = mapper;
            _schoolReportServices = schoolReportServices;

        }


        public async Task<Result<AttendanceReportResponse>> Handle(AttendanceReportQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _schoolReportServices.GetAttendanceReport(request.AttendanceReportDTOs);

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
