using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.Documents.Queries.DocumentsType.FilterDocumentsType
{
    public record FilterDocumentsTypeDTOs
    (
        string? name,
        string? startDate,
        string? endDate
        );
}
