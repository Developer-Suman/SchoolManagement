using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.Documents.Queries.Documents.DocumentsById
{
    public record DocumentsByIdQuery
    (
        string documentsId
        ) : IRequest<Result<DocumentsByIdResponse>>;
}
