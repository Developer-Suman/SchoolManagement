using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.VisaEnum;

namespace ES.Visa.Application.Visa.Queries.VisaStatus.FilterVisaStatus
{
    public record FilterVisaStatusResponse
    (
        string id = "",
            string name = "",
            VisaStatusType visaStatusType = VisaStatusType.Application,
            bool isActive = false,
            string schoolId = "",
            string createdBy = "",
            DateTime createdAt = default,
            string modifiedBy = "",
            DateTime modifiedAt = default
        );
}
