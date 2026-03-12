using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.AddCounselor
{
    public record AddCounselorRequest
    (
            string fullName,
            string? email,
            string contactNumber
        );
    
}
