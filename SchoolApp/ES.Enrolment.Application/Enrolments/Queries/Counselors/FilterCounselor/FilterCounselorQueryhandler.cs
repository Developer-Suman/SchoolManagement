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

namespace ES.Enrolment.Application.Enrolments.Queries.Counselors.FilterCounselor
{
    public class FilterCounselorQueryhandler : IRequestHandler<FilterCounselorQuery, Result<PagedResult<FilterCounselorResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly ICounselorServices _counselorServices;

        public FilterCounselorQueryhandler(IMapper mapper, ICounselorServices counselorServices)
        {
            _mapper = mapper;
            _counselorServices = counselorServices;
            
        }
        public async Task<Result<PagedResult<FilterCounselorResponse>>> Handle(FilterCounselorQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _counselorServices.FilterCounselor(request.paginationRequest, request.filterCounselorDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterCounselorResponse>>(result.Data);

                return Result<PagedResult<FilterCounselorResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterCounselorResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
