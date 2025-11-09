

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdatePaymentTransactionNumberType
{
    public record UpdatePaymentTransactionNumberTypeRequest
    (
        TransactionNumberType transactionNumberType
        );
}
