using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Visa.Application.Visa.Command.VisaApplication.UpdateVisaApplication
{
    public record UpdateVisaApplicationResponse
    (
        string id,
            string applicantId,
            string countryId,
            string universityId,
            string courseId,
            string intakeId,
            DateTime appliedDate,
            string visaStatusId,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            string visaDetails,
            bool emailSent,
            string emailContent,
            List<UpdateVisaApplicationResponseDTOs> updateVisaApplicationResponseDTOs
        );
}
