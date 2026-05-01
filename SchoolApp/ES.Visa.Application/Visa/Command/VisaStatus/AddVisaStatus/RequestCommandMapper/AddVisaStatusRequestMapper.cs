using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Visa.Application.Visa.Command.VisaStatus.AddVisaStatus.RequestCommandMapper
{
    public static class AddVisaStatusRequestMapper
    {
        public static AddVisaStatusCommand ToCommand(this AddVisaStatusRequest addVisaStatusRequest)
        {
            return new AddVisaStatusCommand(
                addVisaStatusRequest.name,
                addVisaStatusRequest.visaStatusType
                );
        }
    }
}
