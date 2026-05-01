using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Visa.Application.Visa.Command.VisaApplication.AddVisaApplication.RequestCommandMapper
{
    public static class AddVisaApplicationRequestMapper
    {
        public static AddVisaApplicationCommand ToCommand(this AddVisaApplicationRequest request)
        {
            return new AddVisaApplicationCommand
                (
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
                request.visaApplicationDocumentsDTOs
                );
        }
    }
}
