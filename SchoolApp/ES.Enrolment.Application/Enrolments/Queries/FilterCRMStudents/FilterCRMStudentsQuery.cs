using ES.Enrolment.Application.Enrolments.Queries.FilterCRMStudents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.FilterCRMStudents
{
    public record FilterCRMStudentsQuery
    (

        PaginationRequest PaginationRequest,
        FilterCRMStudentsDTOs FilterCRMStudentsDTOs
        ) : IRequest<Result<PagedResult<FilterCRMStudentsResponse>>>;
}
