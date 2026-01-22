using ES.Certificate.Application.Certificates.Queries.SchoolAwards.FilterSchoolAwards;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Certificate.Application.Certificates.Queries.StudentsAwards.FilterStudentsAwards
{
    public record FilterStudentsAwardsQuery
        (
        PaginationRequest PaginationRequest,
        FilterStudentsAwardsDTOs FilterStudentsAwardsDTOs
        ) : IRequest<Result<PagedResult<FilterStudentsAwardsResponse>>>;
}
    