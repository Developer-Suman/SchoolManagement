using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Entities.Certificates.IssuedCertificate;

namespace ES.Certificate.Application.Certificates.Command.UpdateIssuedCertificate
{
    public record UpdateIssuedCertificateRequest
   (
        string templateId,
            string studentId,
            string schoolId,
            string certificateNumber,
            DateTime issuedDate,
            string? issuedBy,
            string? pdfPath,
            string? remarks,
            CertificateStatus status,
            DateTime yearOfCompletion,
             string program,
            string symbolNumber
        );
}
