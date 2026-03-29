using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.Documents.Command.UploadApplicantDocuments.RequestCommandMapper
{
    public static class UploadApplicantDocumentsRequestMapper
    {
        public static UploadApplicantDocumentsCommand ToCommand(this UploadApplicantDocumentsRequest request)
        {
            return new UploadApplicantDocumentsCommand
                (
                request.applicantId,
                request.UploadApplicantDocumentsDTOs
                );
        }
    }
}
