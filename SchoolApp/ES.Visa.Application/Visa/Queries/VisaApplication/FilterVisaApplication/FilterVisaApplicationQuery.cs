using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Visa.Application.Visa.Queries.VisaApplication.FilterVisaApplication
{
    public record FilterVisaApplicationQuery
    (
        PaginationRequest PaginationRequest,
                FilterVisaApplicationDTOs FilterVisaApplicationDTOs
        ) : IRequest<Result<PagedResult<FilterVisaApplicationResponse>>>;
    
}
