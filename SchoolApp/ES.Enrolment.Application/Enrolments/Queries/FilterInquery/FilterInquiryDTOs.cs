using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Queries.FilterInquery
{
    public record FilterInquiryDTOs
    (
        string? userId,
        string? startDate,
        string? endDate
        );
}
