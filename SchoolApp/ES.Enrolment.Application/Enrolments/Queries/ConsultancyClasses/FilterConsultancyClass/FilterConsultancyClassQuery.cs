using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.ConsultancyClasses.FilterConsultancyClass
{
    public record FilterConsultancyClassQuery
    (
        FilterConsultancyClassDTOs FilterConsultancyClassDTOs,
        PaginationRequest PaginationRequest
        ) : IRequest<Result<PagedResult<FilterConsultancyClassResponse>>>;
}
