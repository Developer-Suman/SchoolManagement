using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Queries.FilterCertificateTemplate
{
    public record FilterCertificateTemplateResponse
    (
        string id,
            string schoolId,
            string templateName,
            string templateSubject,
            string templateType,
            string htmlTemplate,
            bool isActive,
            int templateVersion,
            DateTime createdAt
        );
}
