using ES.Visa.Application.Visa.Queries.VisaApplicationStatusHistory.FilterVisaApplicationHistory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Visa.Application.Visa.Queries.VisaStatus.FilterVisaStatus
{
    public record FilterVisaStatusQuery
    (
        PaginationRequest PaginationRequest,
        FilterVisaStatusDTOs FilterVisaStatusDTOs
        ) : IRequest<Result<PagedResult<FilterVisaStatusResponse>>>;
}
