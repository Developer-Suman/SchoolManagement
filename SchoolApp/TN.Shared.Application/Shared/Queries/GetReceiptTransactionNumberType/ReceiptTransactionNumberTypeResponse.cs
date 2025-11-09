

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Queries.GetReceiptTransactionNumberType
{
    public record ReceiptTransactionNumberTypeResponse
    (
          TransactionNumberType transactionNumberType = default,
            string schoolId = "",
            string? receiptTransactionNoType = null
        );
}
