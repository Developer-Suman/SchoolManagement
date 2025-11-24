using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Entities.Certificates.IssuedCertificate;

namespace ES.Certificate.Application.Certificates.Queries.FilterIssuedCertificate
{
    public record FilterIssuedCertificateResponse
    (
        string id,
            string templateId,
            string studentId,
            string schoolId,
            string certificateNumber,
            DateTime issuedDate,
            string? issuedBy,
            string? pdfPath,
            string? remarks,
            CertificateStatus status,
            DateTime createdAt,
            DateTime yearOfCompletion,
             string program,
            string symbolNumber,
            string examId
        );
}
