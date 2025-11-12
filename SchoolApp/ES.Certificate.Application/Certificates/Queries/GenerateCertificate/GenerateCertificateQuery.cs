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
        string studentId
        ): IRequest<Result<GenerateCertificateResponse>>;
}
