using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Queries.Counselors.FilterCounselor
{
    public record FilterCounselorDTOs
    (
        string? fullName,
        string? startDate,
        string? endDate
        );
}
