

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Queries.GetIncomeTransactionNumberType
{
    public record  GetIncomeTransactionNumberTypeResponse
    (

         TransactionNumberType transactionNumberType = default,
            string schoolId = "",
            string? incomeTransactionNumberType = ""
    );
}
