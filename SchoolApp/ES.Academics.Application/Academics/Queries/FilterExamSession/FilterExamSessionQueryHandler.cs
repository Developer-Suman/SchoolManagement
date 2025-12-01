using AutoMapper;
using ES.Academics.Application.Academics.Queries.FilterExamResult;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.Academics.Queries.FilterExamSession
{
    public class FilterExamSessionQueryHandler : IRequestHandler<FilterExamSessionQuery, Result<PagedResult<FilterExamSessionResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ISeatPlanningServices _seatPlanningServices;

        public FilterExamSessionQueryHandler(ISeatPlanningServices seatPlanningServices, IMapper mapper)
        {
            _seatPlanningServices = seatPlanningServices;
            _mapper = mapper;
            
        }
        public async Task<Result<PagedResult<FilterExamSessionResponse>>> Handle(FilterExamSessionQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _seatPlanningServices.GetFilterExamSession(request.PaginationRequest, request.FilterExamSessionDTOs);

                var examResult = _mapper.Map<PagedResult<FilterExamSessionResponse>>(result.Data);

                return Result<PagedResult<FilterExamSessionResponse>>.Success(examResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterExamSessionResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
