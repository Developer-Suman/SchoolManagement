using AutoMapper;
using ES.Enrolment.Application.Enrolments.Command.TranningRegistration.AddTranningRegistration;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.Appointments.FilterAppointment
{
    public class FilterAppointmentQueryHandler : IRequestHandler<FilterAppointmentQuery, Result<PagedResult<FilterAppointmentResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IAppointmentServices _appointmentServices;

        public FilterAppointmentQueryHandler(IMapper mapper, IAppointmentServices appointmentServices)
        {
            _mapper = mapper;
            _appointmentServices = appointmentServices;
            
        }
        public async Task<Result<PagedResult<FilterAppointmentResponse>>> Handle(FilterAppointmentQuery request, CancellationToken cancellationToken)
        {
            var entityName = typeof(FilterAppointmentQuery).Name
                   .Replace("Filter", "")
                   .Replace("Query", "");
            try
            {

                var result = await _appointmentServices.FilterAppointments(request.PaginationRequest, request.FilterAppointmentDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterAppointmentResponse>>(result.Data);

                return Result<PagedResult<FilterAppointmentResponse>>.Success(resultDisplay,$"{entityName} returned Successfully");
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterAppointmentResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
