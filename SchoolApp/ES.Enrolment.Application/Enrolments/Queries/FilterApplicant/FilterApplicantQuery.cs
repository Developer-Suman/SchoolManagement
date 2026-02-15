using ES.Enrolment.Application.Enrolments.Queries.FilterInquery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.FilterApplicant
{
    public record FilterApplicantQuery
    (
        PaginationRequest PaginationRequest,
        FilterApplicantDTOs FilterApplicantDTOs
        ) : IRequest<Result<PagedResult<FilterApplicantResponse>>>;
}
