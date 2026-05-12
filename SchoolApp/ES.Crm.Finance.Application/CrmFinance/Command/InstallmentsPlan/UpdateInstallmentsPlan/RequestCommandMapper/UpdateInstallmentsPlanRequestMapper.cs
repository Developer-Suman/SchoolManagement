using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.UpdateInstallmentsPlan.RequestCommandMapper
{
    public static class UpdateInstallmentsPlanRequestMapper
    {
        public static UpdateInstallmentsPlanCommand ToCommand(this UpdateInstallmentsPlanRequest request, string id)
        {
            return new UpdateInstallmentsPlanCommand(
                id,
                request.invoiceId,
                request.numberOfInstallments
            );
        }
    }
}
