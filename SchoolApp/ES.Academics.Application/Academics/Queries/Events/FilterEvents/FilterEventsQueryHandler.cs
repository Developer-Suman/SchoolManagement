using AutoMapper;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.Academics.Queries.Events.FilterEvents
{
    public class FilterEventsQueryHandler : IRequestHandler<FilterEventsQuery, Result<PagedResult<FilterEventsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IEventsServices _eventsServices;
        public FilterEventsQueryHandler(IMapper mapper, IEventsServices eventsServices)
        {
            _eventsServices = eventsServices;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<FilterEventsResponse>>> Handle(FilterEventsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _eventsServices.GetFilterEvents(request.PaginationRequest, request.FilterEventsDTOs);

                var filterEventsResult = _mapper.Map<PagedResult<FilterEventsResponse>>(result.Data);

                return Result<PagedResult<FilterEventsResponse>>.Success(filterEventsResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterEventsResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
