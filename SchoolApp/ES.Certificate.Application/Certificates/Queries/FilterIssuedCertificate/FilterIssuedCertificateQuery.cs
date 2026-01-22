using ES.Certificate.Application.Certificates.Queries.FilterCertificateTemplate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Certificate.Application.Certificates.Queries.FilterIssuedCertificate
{
    public record FilterIssuedCertificateQuery  
    (
        PaginationRequest PaginationRequest,
        FilterIssuedCertificateDTOs FilterIssuedCertificateDTOs
        ) : IRequest<Result<PagedResult<FilterIssuedCertificateResponse>>>;
}
