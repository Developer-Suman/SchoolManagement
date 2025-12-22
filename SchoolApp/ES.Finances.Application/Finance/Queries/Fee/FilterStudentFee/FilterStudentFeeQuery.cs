using ES.Finances.Application.Finance.Queries.Fee.FilterFeeStructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.Finance.Queries.Fee.FilterStudentFee
{
    public record FilterStudentFeeQuery
    (
         PaginationRequest PaginationRequest,
        FilterStudentFeeDTOs FilterStudentFeeDTOs
        ) : IRequest<Result<PagedResult<FilterStudentFeeResponse>>>;
}
