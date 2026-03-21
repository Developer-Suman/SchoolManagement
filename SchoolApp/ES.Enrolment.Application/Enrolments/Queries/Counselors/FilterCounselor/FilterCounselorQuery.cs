using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.Counselors.FilterCounselor
{
    public record FilterCounselorQuery
   (
        PaginationRequest paginationRequest,
        FilterCounselorDTOs filterCounselorDTOs
        ) : IRequest<Result<PagedResult<FilterCounselorResponse>>>;
}
