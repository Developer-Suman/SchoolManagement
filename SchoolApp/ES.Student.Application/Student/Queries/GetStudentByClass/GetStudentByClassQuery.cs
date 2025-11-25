using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.Student.Queries.GetStudentByClass
{
    public record GetStudentByClassQuery
    (
        PaginationRequest paginationRequest,
        string classId
        ): IRequest<Result<PagedResult<GetStudentByClassResponse>>>;
}
