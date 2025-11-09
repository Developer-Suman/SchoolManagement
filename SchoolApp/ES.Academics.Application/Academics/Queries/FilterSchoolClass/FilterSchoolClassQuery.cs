using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.FilterLedger;
using TN.Account.Application.Account.Queries.FilterLedgerByDate;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.Academics.Queries.FilterSchoolClass
{
    public record FilterSchoolClassQuery
     (
        PaginationRequest PaginationRequest,
        FilterSchoolClassDTOs FilterSchoolClassDTOs
    ) : IRequest<Result<PagedResult<FilterSchoolClassQueryResponse>>>;
}
