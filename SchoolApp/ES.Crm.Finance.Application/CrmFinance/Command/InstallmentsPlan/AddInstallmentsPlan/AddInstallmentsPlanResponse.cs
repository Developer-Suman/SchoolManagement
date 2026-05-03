using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan
{
    public record AddInstallmentsPlanResponse
    (
        string id="",
            string invoiceId="",
            int numberOfInstallments=0,
            decimal totalAmount=0,
            bool isActive=false,
            string schoolId="",
            string createdBy="",
            DateTime createdAt=default,
            string modifiedBy="",
            DateTime modifiedAt=default
        );
}
