using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Queries.FilterCertificateTemplate
{
    public record FilterCertificateTemplatesDTOs
    (
        string? schoolId,
        string? startDate,
        string? endDate
        );
}
