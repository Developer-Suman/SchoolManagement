using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.EnrolmentTypeEnum;

namespace ES.Enrolment.Application.Enrolments.Queries.Applicants.FilterApplicant
{
    public record FilterApplicantResponse
    (
        string id ="",
        string userId ="",
        string fullName ="",
        string email ="",
        EnrolmentType enrolmentType = EnrolmentType.Applicant,
        string? passportNo ="",
        string? countryId ="",
        string? countryName ="",
        string? universityId ="",
        string universityName ="",
        string? courseId ="",
            string? courseName="",
            bool isActive=true,
            string schoolId="",
            string createdBy="",
            DateTime createdAt=default,
            string modifiedBy="",
            DateTime modifiedAt=default
        );
}
