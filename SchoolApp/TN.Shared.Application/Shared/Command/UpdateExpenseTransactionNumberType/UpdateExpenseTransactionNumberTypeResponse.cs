

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdateExpenseTransactionNumberType
{
    public record  UpdateExpenseTransactionNumberTypeResponse
    (

          TransactionNumberType transactionNumberType,
            string schoolId
    );
}
