using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.EducationLevelEnum;
using static TN.Shared.Domain.Enum.GenderEnum;

namespace ES.Enrolment.Application.Enrolments.Command.ConvertApplicant
{
    public record ConvertApplicantResponse
   (
        string id="",
           string passportNo="",
           string targetCountry=""
        );
}
