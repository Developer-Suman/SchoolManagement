using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterRequirements;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterUniversity
{
    public record FilterUniversityQuery
    (
        PaginationRequest PaginationRequest,
        FilterUniversityDTOs FilterUniversityDTOs
        ) : IRequest<Result<PagedResult<FilterUniversityResponse>>>;
}
