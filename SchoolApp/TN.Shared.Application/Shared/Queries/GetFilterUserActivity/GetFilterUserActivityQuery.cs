using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Shared.Application.Shared.Queries.GetFilterUserActivity
{
    public record GetFilterUserActivityQuery
    (
        PaginationRequest PaginationRequest,
        UserActivityDTOs UserActivityDTOs
        ) : IRequest<Result<PagedResult<GetFilterUserActivityResponse>>>;
}
