using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.ConvertStudent
{
    public record ConvertStudentResponse
    (
           string universityName,
            string visaId
        );
}
