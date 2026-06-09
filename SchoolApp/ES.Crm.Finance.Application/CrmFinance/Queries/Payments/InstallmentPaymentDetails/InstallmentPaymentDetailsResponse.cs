using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Finance;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Payments.InstallmentPaymentDetails
{
    public record InstallmentPaymentDetailsResponse
    (
        decimal totalAmount,
        int numberOfInstallments,
        decimal baseAmount,
        List<InstallmentPayments> installmentPayments
        );

    public record InstallmentPayments
    (
        decimal paidAmount,
        string paymentDate,
        PaymentMethods paymentMethod,
        string referenceNumber,
        decimal remaingAmount
        );
}
