using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Entities.Certificates.IssuedCertificate;

namespace ES.Certificate.Application.Certificates.Command.AddIssuedCertificate
{
    public record AddIssuedCertificateCommand
    (
         string templateId,
            string studentId,
            string certificateNumber,
            string? remarks,
            CertificateStatus status,
            DateTime yearOfCompletion,
             string program,
            string symbolNumber
        ) : IRequest<Result<AddIssuedCertificateResponse>>;
}
