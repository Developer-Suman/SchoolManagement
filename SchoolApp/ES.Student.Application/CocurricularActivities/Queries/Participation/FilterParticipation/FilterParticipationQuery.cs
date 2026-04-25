
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.CocurricularActivities.Queries.Participation.FilterParticipation
{
    public record FilterParticipationQuery
    (
         FilterParticipationDTOs FilterParticipationDTOs,
        PaginationRequest PaginationRequest
        ) : IRequest<Result<PagedResult<FilterParticipationResponse>>>;
}
