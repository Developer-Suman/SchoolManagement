using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.FilterInstallmentPlan
{
    public record FilterInstallmentPlanDTOs
    (
        string? startDate,
        string? endDate,
        string? invoiceId
        );
}
