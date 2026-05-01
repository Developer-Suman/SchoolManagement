using ES.Visa.Application.Visa.Command.VisaApplication.AddVisaApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Visa.Application.Visa.Queries.VisaApplication.VisaApplication
{
    public record VisaApplicationResponse
    (
        string id = "",
            string applicantId = "",
            string countryId = "",
            string universityId = "",
            string courseId = "",
            string intakeId = "",
            DateTime appliedDate = default,
            string visaStatusId = "",
            List<VisaApplicationDocumentsResponseDTOs> visaApplicationDocumentsDTOs = default
        );
}
