using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Staff.Application.Staff.Queries.FilterAcademicTeam
{
    public record FilterAcademicTeamQuery
   (
        PaginationRequest PaginationRequest,
        FilterAcademicTeamDTOs FilterAcademicTeamDTO
        ) : IRequest<Result<PagedResult<FilterAcademicTeamResponse>>>;
}
