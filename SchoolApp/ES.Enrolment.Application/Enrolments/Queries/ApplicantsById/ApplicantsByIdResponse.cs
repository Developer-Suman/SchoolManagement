using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.EnrolmentTypeEnum;

namespace ES.Enrolment.Application.Enrolments.Queries.ApplicantsById
{
    public record ApplicantsByIdResponse
    (
        string userId,
        string fullName,
        string email,
        EnrolmentType enrolmentType,
        string passportNo,
            string targetCountry,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
        );
}
