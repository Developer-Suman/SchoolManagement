using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.Documents.Command.UploadApplicantDocuments
{
    public record UploadApplicantDocumentsRequest
    (
        string applicantId,
        List<UploadApplicantDocumentsDTOs> UploadApplicantDocumentsDTOs
        );

   
}
