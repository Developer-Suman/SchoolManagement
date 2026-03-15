using AutoMapper;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.Inquiry
{
    public class InquiryQueryHandler : IRequestHandler<InquiryQuery, Result<PagedResult<InquiryResponse>>>
    {
        private readonly IEnrolmentServices _enrolmentServices;
        private readonly IMapper _mapper;


        public InquiryQueryHandler( IEnrolmentServices enrolmentServices, IMapper mapper)
        {
           _enrolmentServices= enrolmentServices;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<InquiryResponse>>> Handle(InquiryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _enrolmentServices.GetAllInquiry(request.PaginationRequest, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching", ex);
            }
        }
    }
}
