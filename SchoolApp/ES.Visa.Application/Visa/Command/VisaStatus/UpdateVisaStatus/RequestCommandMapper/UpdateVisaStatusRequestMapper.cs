using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Visa.Application.Visa.Command.VisaStatus.UpdateVisaStatus.RequestCommandMapper
{
    public static class UpdateVisaStatusRequestMapper
    {
        public static UpdateVisaStatusCommand ToCommand(this UpdateVisaStatusRequest request, string id)
        {
            return new UpdateVisaStatusCommand
            (
                id,
                request.Name,
                request.VisaStatusType
                );
        }
    }
}
