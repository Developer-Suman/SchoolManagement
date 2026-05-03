using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Finance;
using static TN.Shared.Domain.Enum.CrmEnum;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Payments.Addpayments
{
    public record AddPaymentsResponse
    (
        string id="",
            string invoiceId="",
            decimal amount=0,
            DateTime paymentDate=default,
            PaymentMethods paymentMethod=default,
            string referenceNumber="",
            PaymentStatus paymentStatus=default
        );
}
