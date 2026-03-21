using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.EnrolmentTypeEnum;
using static TN.Shared.Domain.Enum.GenderEnum;

namespace ES.Enrolment.Application.Enrolments.Queries.UserProfiles.GetUserProfileById
{
    public record GetUserProfileByIdResponse
    (
          string id = "",
            string fullName="",
            string email="",
            EnrolmentType enrolmentType=EnrolmentType.New,
            Gender? genderStatus = Gender.Male,
            DateTime? dob = default,
            DateTime? admissionDate = default,
            string? intrestedCountry = ""
        );
}
