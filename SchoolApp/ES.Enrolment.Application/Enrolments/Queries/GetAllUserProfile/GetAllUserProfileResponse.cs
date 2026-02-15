using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.EnrolmentTypeEnum;

namespace ES.Enrolment.Application.Enrolments.Queries.GetAllUserProfile
{
    public record GetAllUserProfileResponse
    (
        string id,
            string fullName,
            string email,
            EnrolmentType enrolmentType,
            DateTime createdAt
        );
}
