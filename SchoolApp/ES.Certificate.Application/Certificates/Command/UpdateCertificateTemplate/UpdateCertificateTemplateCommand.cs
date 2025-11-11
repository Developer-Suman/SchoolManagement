using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Command.UpdateCertificateTemplate
{
    public record UpdateCertificateTemplateCommand
    (
        string Id,
             string schoolId,
            string templateName,

            string templateType,
            string htmlTemplate,
            string templateVersion
        ) : IRequest<Result<UpdateCertificateTemplateResponse>>;
}
