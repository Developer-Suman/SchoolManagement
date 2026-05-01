using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Visa.Application.Visa.Command.VisaApplication.UpdateVisaApplication.RequestCommandMapper
{
    public static class UpdateVisaApplicationRequestMapper
    {
        public static UpdateVisaApplicationCommand ToCommand(this UpdateVisaApplicationRequest request, string id)
        {
            return new UpdateVisaApplicationCommand
                (
                id,
                request.applicantId,
                request.countryId,
                request.universityId,
                request.courseId,
                request.intakeId,
                request.appliedDate,
                request.visaStatusId,
                request.visaDetails,
                request.emailSent,
                request.emailContent,
                request.updateVisaApplicationRequestDTOs
                );
        }
    }
}
