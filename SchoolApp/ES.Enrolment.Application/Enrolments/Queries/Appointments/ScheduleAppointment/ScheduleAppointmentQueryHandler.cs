using AutoMapper;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Queries.Appointments.ScheduleAppointment
{
    public class ScheduleAppointmentQueryHandler : IRequestHandler<ScheduleAppointmentQuery, Result<ScheduleAppointmentResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IAppointmentServices _appointmentServices;

        public ScheduleAppointmentQueryHandler(IAppointmentServices appointmentServices, IMapper mapper)
        {
            _mapper = mapper;
            _appointmentServices = appointmentServices;


        }
        public async Task<Result<ScheduleAppointmentResponse>> Handle(ScheduleAppointmentQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _appointmentServices.GetAppointmentSchedule(request.ScheduleAppointmentDTOs);

                var attendanceReport = _mapper.Map<ScheduleAppointmentResponse>(result.Data);

                return Result<ScheduleAppointmentResponse>.Success(attendanceReport);
            }
            catch (Exception ex)
            {
                return Result<ScheduleAppointmentResponse>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
