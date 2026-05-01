using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Visa.Application.Visa.Queries.VisaApplicationStatusHistory.FilterVisaApplicationHistory
{
    public record FilterVisaApplicationStatusHistoryQuery
    (
        PaginationRequest PaginationRequest,
        FilterVisaApplicationStatusHistoryDTOs FilterVisaStatusDTOs
        ) : IRequest<Result<PagedResult<FilterVisaApplicationStatusHistoryResponse>>>;
}
