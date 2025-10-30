

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdateIncomeTransactionNumberTypeCommand
{
    public record  UpdateIncomeTransactionNumberTypeResponse
  (
          TransactionNumberType transactionNumberType,
            string schoolId
        );
}
