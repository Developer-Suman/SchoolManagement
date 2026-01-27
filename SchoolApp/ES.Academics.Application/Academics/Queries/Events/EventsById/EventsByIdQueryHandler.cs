using AutoMapper;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Queries.Events.EventsById
{
    public class EventsByIdQueryHandler : IRequestHandler<EventsByIdQuery, Result<EventsByIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IEventsServices _eventsServices;
        public EventsByIdQueryHandler(IMapper mapper, IEventsServices eventsServices)
        {
            _eventsServices = eventsServices;
            _mapper = mapper;

        }

        public async Task<Result<EventsByIdResponse>> Handle(EventsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var eventsById = await _eventsServices.GetEvents(request.Id);
                return Result<EventsByIdResponse>.Success(eventsById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Events By ID", ex);
            }
        }
    }
}
