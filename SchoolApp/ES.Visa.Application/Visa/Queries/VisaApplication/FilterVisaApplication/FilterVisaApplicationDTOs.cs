using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Visa.Application.Visa.Queries.VisaApplication.FilterVisaApplication
{
    public record FilterVisaApplicationDTOs
    (
        string? startDate,
        string? endDate,
        string? applicantId
        );
}
