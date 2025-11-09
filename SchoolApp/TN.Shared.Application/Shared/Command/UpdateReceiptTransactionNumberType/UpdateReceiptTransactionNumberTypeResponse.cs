

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdateReceiptTransactionNumberType
{
    public record UpdateReceiptTransactionNumberTypeResponse
    (
        TransactionNumberType transactionNumberType,
            string schoolId
        );
    
}
