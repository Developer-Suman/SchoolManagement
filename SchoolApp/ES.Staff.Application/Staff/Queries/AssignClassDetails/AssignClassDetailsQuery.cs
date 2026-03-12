using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Staff.Application.Staff.Queries.AssignClassDetails
{
    public record AssignClassDetailsQuery
    (
        PaginationRequest PaginationRequest,
        AssignClassDetailsDTOs AssignClassDetailsDTOs
        ) : IRequest<Result<PagedResult<AssignClassDetailsResponse>>>;
}
