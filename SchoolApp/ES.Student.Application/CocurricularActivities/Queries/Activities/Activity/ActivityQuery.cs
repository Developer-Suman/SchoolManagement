using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.CocurricularActivities.Queries.Activities.Activity
{
    public record ActivityQuery
   (
        PaginationRequest PaginationRequest
        ) : IRequest<Result<PagedResult<ActivityResponse>>>;
}
