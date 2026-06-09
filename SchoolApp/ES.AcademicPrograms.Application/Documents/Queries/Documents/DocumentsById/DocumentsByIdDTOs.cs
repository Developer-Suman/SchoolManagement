using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.VisaEnum;

namespace ES.AcademicPrograms.Application.Documents.Queries.Documents.DocumentsById
{
    public record DocumentsByIdDTOs
    (
        string documentTypeId,
         DocumentStatus documentStatus,
         string documentsUrl
    );
}
