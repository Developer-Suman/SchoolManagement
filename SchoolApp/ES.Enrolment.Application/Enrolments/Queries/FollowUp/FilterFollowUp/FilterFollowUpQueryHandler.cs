using AutoMapper;
using ES.Enrolment.Application.Enrolments.Queries.Enquiry.FilterInquery;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.FollowUp.FilterFollowUp
{
    public class FilterFollowUpQueryHandler : IRequestHandler<FilterFollowUpQuery, Result<PagedResult<FilterFollowUpResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IFollowUpServices _followUpServices;

        public FilterFollowUpQueryHandler(IMapper mapper, IFollowUpServices followUpServices)
        {
            _mapper = mapper;
            _followUpServices = followUpServices;
            
        }
        public async Task<Result<PagedResult<FilterFollowUpResponse>>> Handle(FilterFollowUpQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _followUpServices.Filter(request.PaginationRequest, request.FilterFollowUpDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterFollowUpResponse>>(result.Data);

                return Result<PagedResult<FilterFollowUpResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterFollowUpResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
