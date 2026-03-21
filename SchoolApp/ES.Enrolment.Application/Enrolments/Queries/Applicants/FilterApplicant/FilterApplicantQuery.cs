using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.Applicants.FilterApplicant
{
    public record FilterApplicantQuery
    (
        PaginationRequest PaginationRequest,
        FilterApplicantDTOs FilterApplicantDTOs
        ) : IRequest<Result<PagedResult<FilterApplicantResponse>>>;
}
