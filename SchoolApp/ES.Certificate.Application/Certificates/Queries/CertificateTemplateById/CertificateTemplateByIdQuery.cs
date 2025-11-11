using ES.Certificate.Application.Certificates.Queries.CertificateTemplate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Queries.CertificateTemplateById
{
    public record CertificateTemplateByIdQuery
    (string id) : IRequest<Result<CertificateTemplateByIdResponse>>;
}
