using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Queries.FilterCounselor
{
    public record FilterCounselorResponse
   (
         string id,
            string fullName,
            string? email,
            string contactNumber,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
        );
}
