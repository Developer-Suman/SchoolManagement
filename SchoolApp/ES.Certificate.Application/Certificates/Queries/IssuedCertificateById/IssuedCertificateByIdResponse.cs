using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Queries.IssuedCertificateById
{
    public record IssuedCertificateByIdResponse
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
            string status,
            DateTime createdAt,
            DateTime yearOfCompletion
        );

}
