using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Finance;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Payments.UpdatePayments
{
    public record UpdatePaymentsCommand
    (
            string id,
        string invoiceId,
            decimal amount,
            DateTime paymentDate,
            PaymentMethods paymentMethod
        ) : IRequest<Result<UpdatePaymentsResponse>>;
}
