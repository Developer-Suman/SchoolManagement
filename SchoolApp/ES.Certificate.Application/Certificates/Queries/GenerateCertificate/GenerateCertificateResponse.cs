using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Queries.GenerateCertificate
{
    public record GenerateCertificateResponse
    (
        string fullName,
        string parentsName,
        int provinceId,
        int districtId,
        int? municipalityId,
        int? vdcId,
        int wardNumber,
        string certificateProgram,
        DateTime yearOfCompletion,
        string percentage,
        string division,
        DateTime dateOfBirth,
        string symbolNumber,
        string registrationNumber,
        DateTime dateOfIssue,
        string StudentImage
        );
}
