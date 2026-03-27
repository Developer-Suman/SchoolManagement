using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.VisaEnum;

namespace ES.AcademicPrograms.Application.Documents.Queries.Documents.FilterDocuments
{
    public record FilterDocumentsResponse
    (
        string id = "",
            string applicantId = "",
            string documentTypeId = "",
            DocumentStatus documentStatus = default,
            string docLink="",
            bool isActive = true,
            string schoolId = "",
            string createdBy = "",
            DateTime createdAt = default,
            string modifiedBy = "",
            DateTime modifiedAt = default
        );
}
