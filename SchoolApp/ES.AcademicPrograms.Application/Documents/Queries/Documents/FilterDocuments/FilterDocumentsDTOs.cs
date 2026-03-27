using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.Documents.Queries.Documents.FilterDocuments
{
    public record FilterDocumentsDTOs
    (
        string? applicantId,
        string? startDate,
        string? endDate
        );
}
