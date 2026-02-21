using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.EducationLevelEnum;
using static TN.Shared.Domain.Enum.EnrolmentTypeEnum;
using static TN.Shared.Domain.Enum.GenderEnum;

namespace ES.Enrolment.Application.Enrolments.Queries.InqueryById
{
    public record InqueryByIdResponse
    (
        string userId,
        string fullName,
        string email,
        EnrolmentType enrolmentType,
         DateTime dateOfBirth,
         
            Gender gender,
            string contactNumber,
            string permanentAddress,
            EducationLevel educationLevel,
            string completionYear,
            string currentGpa,
            string previousAcademicQualification,
            string source,
            string feedBackOrSuggestion
        );
}
