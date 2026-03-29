using ES.AcademicPrograms.Application.AcademicPrograms.Queries.Course;
using ES.AcademicPrograms.Application.Documents.Command.AddDocuments;
using ES.AcademicPrograms.Application.Documents.Command.AddDocumentsType;
using ES.AcademicPrograms.Application.Documents.Command.DocumentCheckList.NonRequiredDocuments;
using ES.AcademicPrograms.Application.Documents.Command.DocumentCheckList.RequiredDocument;
using ES.AcademicPrograms.Application.Documents.Command.UploadApplicantDocuments;
using ES.AcademicPrograms.Application.Documents.Queries.Documents.DocumentsById;
using ES.AcademicPrograms.Application.Documents.Queries.Documents.FilterDocuments;
using ES.AcademicPrograms.Application.Documents.Queries.DocumentsType.FilterDocumentsType;
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
        Task<Result<UploadApplicantDocumentsResponse>> UploadApplicantDocuments(UploadApplicantDocumentsCommand uploadApplicantDocumentsCommand);
        Task<Result<PagedResult<FilterDocumentsResponse>>> FilterDocuments(FilterDocumentsDTOs filterDocumentsDTOs,PaginationRequest paginationRequest);
        Task<Result<DocumentsByIdResponse>> DocumentsById(string documentsId, CancellationToken cancellationToken = default);


        Task<Result<RequiredDocumentsResponse>> Required(string dockCheckListId);
        Task<Result<NonRequiredDocumentsResponse>> NonRequired(string dockCheckListId);


        Task<Result<AddDocumentsTypeResponse>> AddDocumentsType(AddDocumentsTypeCommand addDocumentsTypeCommand);
        Task<Result<PagedResult<FilterDocumentsTypeResponse>>> FilterDocumentsType(FilterDocumentsTypeDTOs filterDocumentsTypeDTOs, PaginationRequest paginationRequest);

        Task<Result<PagedResult<AddDocumentsTypeResponse>>> DocumentsType(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
    }
}
