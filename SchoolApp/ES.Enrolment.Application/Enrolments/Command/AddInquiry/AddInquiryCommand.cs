using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Enum.EducationLevelEnum;
using static TN.Shared.Domain.Enum.GenderEnum;

namespace ES.Enrolment.Application.Enrolments.Command.AddInquiry
{
    public record AddInquiryCommand
    (
        string fullName,
        string email,
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
        ): IRequest<Result<AddInquiryResponse>>;
}
