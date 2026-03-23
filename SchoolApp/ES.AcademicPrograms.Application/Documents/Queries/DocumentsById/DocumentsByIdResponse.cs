using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.VisaEnum;

namespace ES.AcademicPrograms.Application.Documents.Queries.DocumentsById
{
    public record DocumentsByIdResponse
    (
        string id="",
            string applicantId="",
            string documentTypeId = "",
            DocumentStatus documentStatus=default,
            bool isActive=true,
            string schoolId = "",
            string createdBy = "",
            DateTime createdAt=default,
            string modifiedBy = "",
            DateTime modifiedAt = default
        );
}
