using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterIntake;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterRequirements
{
    public record FilterRequirementsQuery
    (
        PaginationRequest PaginationRequest,
        FilterRequirementsDTOs FilterRequirementsDTOs
        ) : IRequest<Result<PagedResult<FilterRequirementsResponse>>>;
}
