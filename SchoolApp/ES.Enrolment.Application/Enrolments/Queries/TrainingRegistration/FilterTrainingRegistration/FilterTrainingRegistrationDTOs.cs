using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Queries.TrainingRegistration.FilterTrainingRegistration
{
    public record FilterTrainingRegistrationDTOs
    (
        string? applicantId,
        string? startDate,
        string? endDate
        );
}
