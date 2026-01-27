using ES.Academics.Application.Academics.Command.Events.AddEvents;
using ES.Academics.Application.Academics.Command.Events.UpdateEvents;
using ES.Academics.Application.Academics.Queries.Events.Events;
using ES.Academics.Application.Academics.Queries.Events.EventsById;
using ES.Academics.Application.Academics.Queries.Events.FilterEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.ServiceInterface
{
    public interface IEventsServices
    {
        Task<Result<AddEventsResponse>> Add(AddEventsCommand addEventsCommand);
        Task<Result<UpdateEventsResponse>> Update(string eventsId, UpdateEventsCommand updateEventsCommand);
        Task<Result<PagedResult<EventsResponse>>> GetAllEvents(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<EventsByIdResponse>> GetEvents(string eventsId, CancellationToken cancellationToken = default);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<PagedResult<FilterEventsResponse>>> GetFilterEvents(PaginationRequest paginationRequest, FilterEventsDTOs filterEventsDTOs);
    }
}
