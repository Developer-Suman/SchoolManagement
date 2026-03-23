using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Enum.VisaEnum;

namespace ES.AcademicPrograms.Application.Documents.Command.AddDocuments
{
    public record AddDocumentsCommand
    (
        string applicantId,
            string documentTypeId,
            DocumentStatus documentStatus
        ) : IRequest<Result<AddDocumentsResponse>>;
}
