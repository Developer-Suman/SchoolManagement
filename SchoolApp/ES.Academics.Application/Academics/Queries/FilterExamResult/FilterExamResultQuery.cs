using ES.Academics.Application.Academics.Queries.FilterExam;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.Academics.Queries.FilterExamResult
{
    public record FilterExamResultQuery
    (
        PaginationRequest PaginationRequest,
        FilterExamResultDTOs FilterExamResultDTOs
        ) : IRequest<Result<PagedResult<FilterExamResultResponse>>>;
}
