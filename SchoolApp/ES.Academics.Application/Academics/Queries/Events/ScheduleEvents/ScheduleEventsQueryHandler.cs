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

namespace ES.Academics.Application.Academics.Queries.Events.ScheduleEvents
{
    public class ScheduleEventsQueryHandler : IRequestHandler<ScheduleEventsQuery, Result<ScheduleEventsResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IEventsServices _eventsServices;

        public ScheduleEventsQueryHandler(IMapper mapper, IEventsServices eventsServices)
        {
            _eventsServices = eventsServices;
            _mapper = mapper;
        }
        public async Task<Result<ScheduleEventsResponse>> Handle(ScheduleEventsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _eventsServices.GetEventsSchedule(request.scheduleEventsDTOs);

                var resultDisplay = _mapper.Map<ScheduleEventsResponse>(result.Data);

                return Result<ScheduleEventsResponse>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<ScheduleEventsResponse>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
