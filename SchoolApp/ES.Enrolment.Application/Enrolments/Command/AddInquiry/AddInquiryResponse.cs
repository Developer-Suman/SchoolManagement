using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.EducationLevelEnum;
using static TN.Shared.Domain.Enum.GenderEnum;

namespace ES.Enrolment.Application.Enrolments.Command.AddInquiry
{
    public record AddInquiryResponse
    (
        string id="",
            string fullName="",
            DateTime dateOfBirth=default,
            Gender gender= Gender.Male,
            string contactNumber="",
            string permanentAddress = "",
            EducationLevel educationLevel= EducationLevel.Bachelors,
            string completionYear="",
            string currentGpa = "",
            string previousAcademicQualification = "",
            string source = "",
            string feedBackOrSuggestion=""
        );
}
