using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItems
{
    public record FilterSchoolItemsQuery
    (
        PaginationRequest PaginationRequest,
        FilterSchoolItemsDTOs filterSchoolItemsDTOs
        ) : IRequest<Result<PagedResult<FilterSchoolItemsQueryResponse>>>;
}
