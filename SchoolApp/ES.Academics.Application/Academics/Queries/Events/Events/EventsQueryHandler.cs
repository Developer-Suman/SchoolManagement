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
using ZXing;

namespace ES.Academics.Application.Academics.Queries.Events.Events
{
    public class EventsQueryHandler : IRequestHandler<EventsQuery, Result<PagedResult<EventsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IEventsServices _eventsServices;
        public EventsQueryHandler(IMapper mapper, IEventsServices eventsServices)
        {
            _eventsServices = eventsServices;
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<EventsResponse>>> Handle(EventsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var events = await _eventsServices.GetAllEvents(request.PaginationRequest);
                var eventsResult = _mapper.Map<PagedResult<EventsResponse>>(events.Data);
                return Result<PagedResult<EventsResponse>>.Success(eventsResult);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching all Events", ex);
            }
        }
    }
}
