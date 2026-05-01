using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Visa.Application.Visa.Queries.VisaApplicationStatusHistory.FilterVisaApplicationHistory
{
    public record FilterVisaApplicationStatusHistoryDTOs
    (
        string? startDate,
        string? endDate,
        string? visaStatusId
        );
}
