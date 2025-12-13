using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.AddIssuedCertificate.RequestCommandMapper
{
    public static class AddIssuedCertificateRequestMapper
    {
        public static AddIssuedCertificateCommand ToCommand(this AddIssuedCertificateRequest addIssuedCertificateRequest)
        {
            return new AddIssuedCertificateCommand
                (
                addIssuedCertificateRequest.templateId,
                addIssuedCertificateRequest.studentId,
                addIssuedCertificateRequest.certificateNumber,
                addIssuedCertificateRequest.remarks,
                addIssuedCertificateRequest.status,
                addIssuedCertificateRequest.yearOfCompletion,
                addIssuedCertificateRequest.program,
                addIssuedCertificateRequest.symbolNumber
                );
        }
    }
}
