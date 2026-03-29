using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.VisaEnum;

namespace ES.AcademicPrograms.Application.Documents.Command.UploadApplicantDocuments
{
    public record UploadApplicantDocumentsDTOs
    (
         string documentTypeId,
         DocumentStatus documentStatus,
         IFormFile documents
        );

}
