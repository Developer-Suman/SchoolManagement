using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.Documents.Queries.FilterDocuments
{
    public record FilterDocumentsQuery
    (
        PaginationRequest PaginationRequest,
        FilterDocumentsDTOs FilterDocumentsDTOs
        ) : IRequest<Result<PagedResult<FilterDocumentsResponse>>>;
}
