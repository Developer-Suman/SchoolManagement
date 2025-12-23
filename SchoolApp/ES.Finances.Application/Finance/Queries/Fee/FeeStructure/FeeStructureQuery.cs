using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.Finance.Queries.Fee.FeeStructure
{
    public record FeeStructureQuery
    (
        PaginationRequest PaginationRequest
        ) : IRequest<Result<PagedResult<FeeStructureResponse>>>;
}
