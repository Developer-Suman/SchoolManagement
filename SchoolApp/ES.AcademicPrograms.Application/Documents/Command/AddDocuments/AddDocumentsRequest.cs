using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.VisaEnum;

namespace ES.AcademicPrograms.Application.Documents.Command.AddDocuments
{
    public record AddDocumentsRequest
    (
        string applicantId,
            string documentTypeId,
            DocumentStatus documentStatus
        );
}
