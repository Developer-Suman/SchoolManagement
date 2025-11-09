

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Queries.GetExpenseTransactionNumberType
{
    public record  GetExpenseTransactionNumberTypeResponse
    (

         TransactionNumberType transactionNumberType = default,
            string schoolId = "",
            string? expensesTransactionNumberType = ""


    );
}
