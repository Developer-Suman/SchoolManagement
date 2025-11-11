using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Command.DeleteCertificateTemplate
{
    public record DeleteCertificateTemplateCommand(string id) : IRequest<Result<bool>>;

}
