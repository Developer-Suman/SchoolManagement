using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.Finance.Queries.Fee.FeeStructureByClass
{
    public record FeeStructureByClassQuery
    (
        PaginationRequest paginationRequest,
        FeeStructureByClassDTOs FeeStructureByClassDTOs
        ): IRequest<Result<PagedResult<FeeStructureByClassResponse>>>;
}
