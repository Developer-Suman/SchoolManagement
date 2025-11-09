using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdateReceiptTransactionNumberType
{
    public record UpdateReceiptTransactionNumberTypeCommand
    (
        TransactionNumberType transactionNumberType,
        string schoolId
        ) : IRequest<Result<UpdateReceiptTransactionNumberTypeResponse>>;
    
}
