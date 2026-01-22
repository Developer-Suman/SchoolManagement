using ES.Certificate.Application.Certificates.Queries.FilterIssuedCertificate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using ZXing;

namespace ES.Certificate.Application.Certificates.Queries.SchoolAwards.FilterSchoolAwards
{
    public record FilterSchoolAwardsQuery
    (
        PaginationRequest PaginationRequest,
        FilterSchoolAwardsDTOs FilterSchoolAwardsDTOs
        ) : IRequest<Result<PagedResult<FilterSchoolAwardsResponse>>>;
}
