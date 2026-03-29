using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.Documents.Command.UploadApplicantDocuments
{
    public record UploadApplicantDocumentsResponse
    (
         string applicantId="",
        List<UploadApplicantDocumentsDTOs> UploadApplicantDocumentsDTOs=default
        );
}
