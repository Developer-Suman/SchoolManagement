using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.UpdateInstallmentsPlan
{
    public record UpdateInstallmentsPlanRequest
    (
            string invoiceId,
            int numberOfInstallments
        );
}
