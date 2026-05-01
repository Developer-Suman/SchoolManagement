using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Visa.Application.Visa.Queries.VisaStatus.FilterVisaStatus
{
    public record FilterVisaStatusDTOs
    (
        string? startDate,
        string? endDate,
        string? name
        );
}
