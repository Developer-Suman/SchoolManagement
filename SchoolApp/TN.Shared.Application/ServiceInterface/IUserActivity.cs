using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.Shared.Queries.GetFilterUserActivity;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Shared.Application.ServiceInterface
{
    public interface IUserActivity
    {
        Task<Result<PagedResult<GetFilterUserActivityResponse>>> GetFilterUserActivity(PaginationRequest paginationRequest, UserActivityDTOs userActivityDTOs);
    }
}
