using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.FilterInquery
{
    public record FilterInquiryQuery
    (
          PaginationRequest PaginationRequest,
        FilterInquiryDTOs FilterInquiryDTOs
        ): IRequest<Result<PagedResult<FilterInqueryResponse>>>;
}
