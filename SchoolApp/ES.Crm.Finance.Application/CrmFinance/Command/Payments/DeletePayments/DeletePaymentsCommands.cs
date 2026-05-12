using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Payments.DeletePayments
{
    public record DeletePaymentsCommands
    (
        string Id
        ) : IRequest<Result<bool>>;
}
