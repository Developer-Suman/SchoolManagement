using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Queries.FilterIssuedCertificate
{
    public record FilterIssuedCertificateDTOs
    (
        string? templateId,
        string? startDate,
        string? endDate
        );
}
