using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Finance;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Payments.UpdatePayments
{
    public record UpdatePaymentsRequest
    (
        string invoiceId,
            decimal amount,
            string paymentDate,
            PaymentMethods paymentMethod
        );
}
