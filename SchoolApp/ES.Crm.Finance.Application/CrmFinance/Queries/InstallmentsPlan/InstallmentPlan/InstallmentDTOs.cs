using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.InstallmentPlan
{
    public record InstallmentDTOs
    (
        string installmentPlanId,
        decimal amount,
        DateTime dueDate,
        bool isPaid
        );
}
