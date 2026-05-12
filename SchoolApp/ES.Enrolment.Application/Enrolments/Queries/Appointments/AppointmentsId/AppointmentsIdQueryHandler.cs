using AutoMapper;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Queries.Appointments.AppointmentsId
{
    public class AppointmentsIdQueryHandler : IRequestHandler<AppointmentsIdQuery, Result<AppointmentsIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IAppointmentServices _appointmentServices;

        public AppointmentsIdQueryHandler(IAppointmentServices appointmentServices, IMapper mapper)
        {
            _mapper = mapper;
            _appointmentServices = appointmentServices;

        }
        public async Task<Result<AppointmentsIdResponse>> Handle(AppointmentsIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var query = await _appointmentServices.Get(request.id);
                return Result<AppointmentsIdResponse>.Success(query.Data);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
