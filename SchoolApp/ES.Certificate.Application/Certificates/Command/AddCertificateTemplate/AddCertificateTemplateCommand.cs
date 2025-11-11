using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Command.AddCertificateTemplate
{
    public record AddCertificateTemplateCommand
    (
            string templateName,

            string templateType,
            string htmlTemplate,
            string templateVersion
        ) : IRequest<Result<AddCertificateTemplateResponse>>;
}
