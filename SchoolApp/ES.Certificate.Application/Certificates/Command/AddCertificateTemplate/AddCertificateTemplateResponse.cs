using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.AddCertificateTemplate
{
    public record AddCertificateTemplateResponse
    (
        string id,
            string schoolId,
            string templateName,
            string templateSubject,
            string templateType,
            string htmlTemplate,
            bool isActive,
            string templateVersion,
            DateTime createdAt
        );
}
