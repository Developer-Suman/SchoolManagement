using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Payments.InstallmentPaymentDetails
{
    public record InstallmentPaymentDetailsDTOs
   (
        string invoiceid,
        string? startDate,
        string? endDate

        );
}
