

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdateIncomeTransactionNumberTypeCommand
{
    public record  UpdateIncomeTransactionNumberTypeRequest
   (
         TransactionNumberType transactionNumberType

    );
}
