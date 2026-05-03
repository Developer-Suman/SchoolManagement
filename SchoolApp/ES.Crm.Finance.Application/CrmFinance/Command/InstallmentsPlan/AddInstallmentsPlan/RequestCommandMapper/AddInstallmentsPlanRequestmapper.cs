using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan.RequestCommandMapper
{
    public static class AddInstallmentsPlanRequestmapper
    {
        public static AddInstallmentsPlanCommand ToCommand(this AddInstallmentsPlanRequest request)
        {
            return new AddInstallmentsPlanCommand(
                request.invoiceId,
                request.numberOfInstallments
                //request.installmentsDTOs
                );
        }
    }
}
