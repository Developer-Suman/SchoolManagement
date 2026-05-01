using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.VisaEnum;

namespace ES.Visa.Application.Visa.Command.VisaStatus.AddVisaStatus
{
    public record AddVisaStatusRequest
    (
        string name,
            VisaStatusType visaStatusType
        );
}
