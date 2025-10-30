

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Queries.GetPaymentTransactionNumberType
{
    public record GetPaymentTransactionNumberTypeResponse
   (
          TransactionNumberType transactionNumberType = default,
            string SchoolId = "",
            string? paymentTransactionNoType = ""
        );
}
