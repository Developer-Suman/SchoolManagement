using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.Documents.Command.UploadApplicantDocuments
{
    public record UploadApplicantDocumentsCommand
    (
        string applicantId,
        List<UploadApplicantDocumentsDTOs> UploadApplicantDocumentsDTOs
        ) : IRequest<Result<UploadApplicantDocumentsResponse>>;
}
