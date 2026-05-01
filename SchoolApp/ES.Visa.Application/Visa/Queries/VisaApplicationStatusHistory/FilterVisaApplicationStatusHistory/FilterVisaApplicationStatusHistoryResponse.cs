using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.VisaEnum;

namespace ES.Visa.Application.Visa.Queries.VisaApplicationStatusHistory.FilterVisaApplicationHistory
{
    public record FilterVisaApplicationStatusHistoryResponse
    (
         string id="",
            string visaApplicationId="",
            string visaStatusId="",
            string? remarks="",
            DateTime changedAt=default,
            string fyId="",
            bool isActive=false,
            string schoolId="",
            string createdBy="",
            DateTime createdAt=default,
            string modifiedBy="",
            DateTime modifiedAt=default
        );
}
