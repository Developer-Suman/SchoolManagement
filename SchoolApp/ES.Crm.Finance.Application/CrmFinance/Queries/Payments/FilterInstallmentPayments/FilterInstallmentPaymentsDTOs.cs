using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Payments.FilterInstallmentPayments
{
    public record FilterInstallmentPaymentsDTOs
   (
                string? startDate,
        string? endDate,
        string? invoiceId
        );
}
