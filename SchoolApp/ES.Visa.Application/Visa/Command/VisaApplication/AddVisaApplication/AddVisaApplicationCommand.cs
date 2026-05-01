using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Visa.Application.Visa.Command.VisaApplication.AddVisaApplication
{
    public record AddVisaApplicationCommand
    (
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
            List<VisaApplicationDocumentsDTOs> visaApplicationDocumentsDTOs
        ) : IRequest<Result<AddVisaApplicationResponse>>;
}
