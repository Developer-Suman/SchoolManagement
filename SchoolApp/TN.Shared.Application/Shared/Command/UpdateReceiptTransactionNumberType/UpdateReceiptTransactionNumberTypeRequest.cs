

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdateReceiptTransactionNumberType
{
    public record UpdateReceiptTransactionNumberTypeRequest
    (
        TransactionNumberType transactionNumberType
        );
}
