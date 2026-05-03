using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Finance;
using static TN.Shared.Domain.Enum.CrmEnum;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Payments.Addpayments
{
    public record AddPaymentsRequest
    (
        string invoiceId,
            decimal amount,
            DateTime paymentDate,
            PaymentMethods paymentMethod,
            string referenceNumber,
            PaymentStatus paymentStatus
        );
}
