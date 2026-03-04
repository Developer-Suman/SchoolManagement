using AutoMapper;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Queries.Attendance.AttendanceReport;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.Student.Queries.Attendance.AttendanceCountByStudent
{
    public class AttendanceCountByStudentQueryHandler : IRequestHandler<AttendanceCountByStudentQuery, Result<AttendanceCountByStudentResponse>>
    {

        private readonly IMapper _mapper;
        private readonly IAttendanceServices _attendanceServices;

        public AttendanceCountByStudentQueryHandler(IAttendanceServices attendanceServices, IMapper mapper)
        {
            _mapper = mapper;
            _attendanceServices = attendanceServices;
            
        }

        public async Task<Result<AttendanceCountByStudentResponse>> Handle(AttendanceCountByStudentQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _attendanceServices.GetAttendanceCount(request.AttendanceCountByStudentsDTOs);

                var attendanceCount = _mapper.Map<AttendanceCountByStudentResponse>(result.Data);

                return Result<AttendanceCountByStudentResponse>.Success(attendanceCount);
            }
            catch (Exception ex)
            {
                return Result<AttendanceCountByStudentResponse>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
