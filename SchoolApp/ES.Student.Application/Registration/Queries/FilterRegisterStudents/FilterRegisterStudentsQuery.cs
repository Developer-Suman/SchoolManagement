using ES.Student.Application.Student.Queries.FilterParents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.Registration.Queries.FilterRegisterStudents
{
    public record FilterRegisterStudentsQuery
    (
        PaginationRequest PaginationRequest,
        FilterRegisterStudentsDTOs FilterRegisterStudentsDTOs
        ) : IRequest<Result<PagedResult<FilterRegisterStudentsResponse>>>;
}
