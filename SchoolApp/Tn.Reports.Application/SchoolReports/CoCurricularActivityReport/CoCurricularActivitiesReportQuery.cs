using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.SchoolReports.CoCurricularActivityReport
{
    public record CoCurricularActivitiesReportQuery
    (
        PaginationRequest paginationRequest,
        CoCurricularActivitiesReportDTOs coCurricularActivitiesDTOs
        ) : IRequest<Result<PagedResult<CoCurricularActivitiesReportResponse>>>;
}
