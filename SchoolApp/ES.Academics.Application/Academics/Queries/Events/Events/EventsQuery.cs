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
    public record EventsQuery
    (
       PaginationRequest PaginationRequest
        ): IRequest<Result<PagedResult<EventsResponse>>>;
}
