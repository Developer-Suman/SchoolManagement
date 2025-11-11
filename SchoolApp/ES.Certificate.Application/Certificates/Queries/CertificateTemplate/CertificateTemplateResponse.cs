using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Queries.CertificateTemplate
{
    public record CertificateTemplateResponse
   (
          string id,
            string schoolId,
            string templateName,

            string templateType,
            string htmlTemplate,
            bool isActive,
            string templateVersion,
            DateTime createdAt
        );
}
