using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan
{
    public record AddInstallmentsPlanRequest
    (
        string invoiceId,
            int numberOfInstallments
        );
}
