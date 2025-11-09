
using MediatR;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Authentication.Domain.Entities.SchoolSettings;


namespace TN.Shared.Application.Shared.Command.UpdatePaymentTransactionNumberType
{
    public record UpdatePaymentTransactionNumberTypeCommand
    (
        TransactionNumberType transactionNumberType,
        string schoolId
        ) : IRequest<Result<UpdatePaymentTransactionNumberTypeResponse>>;


}
