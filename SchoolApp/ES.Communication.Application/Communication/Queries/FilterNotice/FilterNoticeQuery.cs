using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Communication.Application.Communication.Queries.FilterNotice
{
    public record FilterNoticeQuery
    (
        PaginationRequest PaginationRequest,
        FilterNoticeDTOs FilterNoticeDTOs
        ) : IRequest<Result<PagedResult<FilterNoticeResponse>>>;
}
