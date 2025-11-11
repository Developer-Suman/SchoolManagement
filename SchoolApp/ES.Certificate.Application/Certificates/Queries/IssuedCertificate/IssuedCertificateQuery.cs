using ES.Certificate.Application.Certificates.Queries.CertificateTemplate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Certificate.Application.Certificates.Queries.IssuedCertificate
{
    public record IssuedCertificateQuery
    (PaginationRequest PaginationRequest

        ) : IRequest<Result<PagedResult<IssuedCertificateResponse>>>;
}
