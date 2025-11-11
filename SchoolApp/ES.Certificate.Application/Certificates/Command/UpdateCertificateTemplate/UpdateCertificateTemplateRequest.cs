using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.UpdateCertificateTemplate
{
    public record UpdateCertificateTemplateRequest
    (
          string schoolId,
            string templateName,

            string templateType,
            string htmlTemplate,
            string templateVersion
        );
}
