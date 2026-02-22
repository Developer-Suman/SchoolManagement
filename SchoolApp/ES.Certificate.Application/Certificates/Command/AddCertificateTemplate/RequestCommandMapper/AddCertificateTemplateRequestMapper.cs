using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.AddCertificateTemplate.RequestCommandMapper
{
    public static class AddCertificateTemplateRequestMapper
    {
        public static AddCertificateTemplateCommand ToCommand(this AddCertificateTemplateRequest addCertificateTemplateRequest)
        {
            return new AddCertificateTemplateCommand(
                addCertificateTemplateRequest.templateName,
                addCertificateTemplateRequest.templateSubject,
                addCertificateTemplateRequest.templateType,
                addCertificateTemplateRequest.htmlTemplate,
                addCertificateTemplateRequest.templateVersion
                );
        }
    }
}
