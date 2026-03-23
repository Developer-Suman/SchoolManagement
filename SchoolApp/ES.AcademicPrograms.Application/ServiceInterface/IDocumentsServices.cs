using ES.AcademicPrograms.Application.Documents.Command.AddDocuments;
using ES.AcademicPrograms.Application.Documents.Command.AddDocumentsType;
using ES.AcademicPrograms.Application.Documents.Queries.DocumentsById;
using ES.AcademicPrograms.Application.Documents.Queries.FilterDocuments;
using ES.AcademicPrograms.Application.Documents.Queries.FilterDocumentsType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.ServiceInterface
{
    public interface IDocumentsServices
    {
        Task<Result<AddDocumentsResponse>> AddDocuments(AddDocumentsCommand addDocumentsCommand);
        Task<Result<PagedResult<FilterDocumentsResponse>>> FilterDocuments(FilterDocumentsDTOs filterDocumentsDTOs,PaginationRequest paginationRequest);
        Task<Result<DocumentsByIdResponse>> DocumentsById(string documentsId, CancellationToken cancellationToken = default);

        Task<Result<AddDocumentsTypeResponse>> AddDocumentsType(AddDocumentsTypeCommand addDocumentsTypeCommand);
        Task<Result<PagedResult<FilterDocumentsTypeResponse>>> FilterDocumentsType(FilterDocumentsTypeDTOs filterDocumentsTypeDTOs, PaginationRequest paginationRequest);
    }
}
