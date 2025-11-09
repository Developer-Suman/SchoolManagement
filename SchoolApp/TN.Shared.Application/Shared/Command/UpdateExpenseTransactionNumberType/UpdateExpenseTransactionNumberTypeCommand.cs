
using MediatR;
using TN.Shared.Domain.Abstractions;
using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdateExpenseTransactionNumberType
{
    public record  UpdateExpenseTransactionNumberTypeCommand
   (
          TransactionNumberType transactionNumberType,
        string schoolId

    ) :IRequest<Result<UpdateExpenseTransactionNumberTypeResponse>>;
}
