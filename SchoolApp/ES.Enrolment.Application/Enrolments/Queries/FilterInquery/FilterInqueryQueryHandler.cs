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

namespace ES.Enrolment.Application.Enrolments.Queries.FilterInquery
{
    public class FilterInqueryQueryHandler : IRequestHandler<FilterInquiryQuery, Result<PagedResult<FilterInqueryResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IEnrolmentServices _enrolmentServices;

        public FilterInqueryQueryHandler(IMapper mapper, IEnrolmentServices feeStructureServices)
        {
            _mapper = mapper;
            _enrolmentServices = feeStructureServices;
        }
        public async Task<Result<PagedResult<FilterInqueryResponse>>> Handle(FilterInquiryQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _enrolmentServices.FilterInquery(request.PaginationRequest, request.FilterInquiryDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterInqueryResponse>>(result.Data);

                return Result<PagedResult<FilterInqueryResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterInqueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
