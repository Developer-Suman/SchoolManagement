using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.UpdateCertificateTemplate
{
    public record UpdateCertificateTemplateResponse
    (
        string id,
            string schoolId,
            string templateName,

            string templateType,
            string htmlTemplate,
            bool isActive,
            int templateVersion,
            DateTime createdAt
        );
}
