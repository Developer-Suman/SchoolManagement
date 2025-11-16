using ES.Academics.Application.Academics.Queries.MarkSheetByStudent;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Queries.GenerateCertificate
{
    public record GenerateCertificateQuery
    (
        MarksSheetDTOs MarksSheetDTOs
        ) : IRequest<Result<GenerateCertificateResponse>>;
}
