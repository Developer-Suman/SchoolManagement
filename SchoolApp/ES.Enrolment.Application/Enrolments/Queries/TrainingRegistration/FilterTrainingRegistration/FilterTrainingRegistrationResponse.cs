using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Queries.TrainingRegistration.FilterTrainingRegistration
{
    public record FilterTrainingRegistrationResponse
    (
        string id = "",
        string applicantId = "",
        string consultancyClassId = "",
        DateTime registeredAt = default,
        bool isActive = true,
        string schoolId = "",
        string createdBy = "",
        DateTime createdAt = default,
        string modifiedBy = "",
        DateTime modifiedAt = default
        );
}
