using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Finance;
using static TN.Shared.Domain.Enum.CrmEnum;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Payments.FilterInstallmentPayments
{
    public record FilterInstallmentPaymentsResponse
    (
        string id = "",
           string invoiceNumber = "",
        string applicantName = "",
            string invoiceId = "",
            decimal amount = 0,
            string paymentDate = default,
            PaymentMethods paymentMethod = default,
            string referenceNumber = "",
            PaymentStatus paymentStatus = default,
            bool isActive = false,
            string schoolId = "",
            string createdBy = "",
            DateTime createdAt = default,
            string modifiedBy = "",
            DateTime modifiedAt = default
        );
}
