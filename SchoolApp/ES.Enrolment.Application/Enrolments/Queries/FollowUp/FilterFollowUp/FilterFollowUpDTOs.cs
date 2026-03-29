using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Queries.FollowUp.FilterFollowUp
{
    public record FilterFollowUpDTOs
    (
          string? userId,
        string? startDate,
        string? endDate
        );
}
