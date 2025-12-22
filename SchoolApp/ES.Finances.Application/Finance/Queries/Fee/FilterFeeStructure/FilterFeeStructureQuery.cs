using ES.Finances.Application.Finance.Queries.Fee.FilterFeetype;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.Finance.Queries.Fee.FilterFeeStructure
{
    public record FilterFeeStructureQuery
    (
         PaginationRequest PaginationRequest,
        FilterFeeStructureDTOs FilterFeeStructureDTOs
        ) : IRequest<Result<PagedResult<FilterFeeStructureResponse>>>;
}
