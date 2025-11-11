using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.UpdateCertificateTemplate.RequestCommandMapper
{
    public static class UpdateCertificateTemplateRequestMapper
    {
        public static UpdateCertificateTemplateCommand ToCommand(this  UpdateCertificateTemplateRequest request, string Id)
        {
            return new UpdateCertificateTemplateCommand(
                Id,
                request.schoolId,
                request.templateName,
                request.templateType,
                request.htmlTemplate,
                request.templateVersion
                );
        }
    }
}
