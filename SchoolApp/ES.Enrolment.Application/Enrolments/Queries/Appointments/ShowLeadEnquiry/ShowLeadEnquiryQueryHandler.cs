using AutoMapper;
using ES.Enrolment.Application.Enrolments.Queries.Appointments.FilterAppointment;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.Appointments.ShowLeadEnquiry
{
    public class ShowLeadEnquiryQueryHandler : IRequestHandler<ShowLeadEnquiryQuery, Result<ShowLeadEnquiryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IAppointmentServices _appointmentServices;

        public ShowLeadEnquiryQueryHandler(IMapper mapper, IAppointmentServices appointmentServices)
        {
            _mapper = mapper;
            _appointmentServices = appointmentServices;

        }
        public async Task<Result<ShowLeadEnquiryResponse>> Handle(ShowLeadEnquiryQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _appointmentServices.ShowLeadEnqueries(request.ShowLeadEnquiryDTOs);

                var resultDetails = new ShowLeadEnquiryResponse
                    (
                      result.Data.Countries
                    );
                //var resultDisplay = _mapper.Map<PagedResult<ShowLeadEnquiryResponse>>(result.Data);

                return Result<ShowLeadEnquiryResponse>.Success(resultDetails);
            }
            catch (Exception ex)
            {
                return Result<ShowLeadEnquiryResponse>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
