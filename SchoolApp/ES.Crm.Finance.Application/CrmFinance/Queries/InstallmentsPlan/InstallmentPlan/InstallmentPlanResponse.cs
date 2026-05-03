using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.InstallmentPlan
{
    public record InstallmentPlanResponse
    (
        string id = "",
            string invoiceId = "",
            int numberOfInstallments = 0,
            decimal totalAmount = 0,
            List<InstallmentDTOs> InstallmentDTOs=default,
            bool isActive = false,
            string schoolId = "",
            string createdBy = "",
            DateTime createdAt = default,
            string modifiedBy = "",
            DateTime modifiedAt = default
        );
}
