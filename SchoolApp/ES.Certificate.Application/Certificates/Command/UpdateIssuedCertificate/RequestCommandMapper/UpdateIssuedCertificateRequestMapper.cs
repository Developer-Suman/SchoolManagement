using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.UpdateIssuedCertificate.RequestCommandMapper
{
    public static class UpdateIssuedCertificateRequestMapper
    {
        public static UpdateIssuedCertificateCommand ToCommand(this UpdateIssuedCertificateRequest request, string id)
        {
            return new UpdateIssuedCertificateCommand
                (
                id,
                request.templateId,
                request.studentId,
                request.certificateNumber,
                request.issuedDate,
                request.issuedBy,
                request.pdfPath,
                request.remarks,
                request.status,
                request.yearOfCompletion
                
                );
        }

    }
}
