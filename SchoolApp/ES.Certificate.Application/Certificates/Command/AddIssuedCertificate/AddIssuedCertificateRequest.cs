using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Entities.Certificates.IssuedCertificate;

namespace ES.Certificate.Application.Certificates.Command.AddIssuedCertificate
{
    public record AddIssuedCertificateRequest
    (
        string templateId,
            string studentId,
            string certificateNumber,
            DateTime issuedDate,
            string? issuedBy,
            string? pdfPath,
            string? remarks,
            CertificateStatus status,
            DateTime yearOfCompletion
        );


  
}
