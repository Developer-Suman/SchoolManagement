using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Visa.Application.Visa.Command.VisaApplication.UpdateVisaApplication
{
    public record UpdateVisaApplicationCommand
    (
        string id,
         string applicantId,
            string countryId,
            string universityId,
            string courseId,
            string intakeId,
            DateTime appliedDate,
            string visaStatusId,
            string visaDetails,
            bool emailSent,
            string emailContent,
            List<UpdateVisaApplicationRequestDTOs> updateVisaApplicationRequestDTOs
        ): IRequest<Result<UpdateVisaApplicationResponse>>;
}
