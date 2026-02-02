using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.ConvertApplicant
{
    public record ConvertApplicantRequest
    (
        string userId,
        string passportNo,
        string targetCountry
        );
}
