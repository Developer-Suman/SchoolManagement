using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.VisaEnum;

namespace ES.AcademicPrograms.Application.Documents.Queries.Documents.DocumentsById
{
    public record DocumentsByIdResponse
    (
        string id="",
            string applicantId="",
            DocumentsByIdDTOs documentsByIdDTOs= default
        );
}
