using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.VisaEnum;

namespace ES.Visa.Application.Visa.Command.VisaStatus.UpdateVisaStatus
{
    public record UpdateVisaStatusResponse
    (
        string id,
            string name,
            VisaStatusType visaStatusType,
            string fyId,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
        );
}
