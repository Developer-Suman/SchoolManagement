using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Finance;

namespace ES.Finances.Application.Finance.Command.PaymentRecords.AddpaymentsRecords
{
    public record AddPaymentsRecordsCommand
    (
        string studentid,
        string classid,
            decimal amountPaid,
            DateTime paymentDate,
            PaymentMethods paymentMethod,
            string reference
        ) : IRequest<Result<AddpaymentsRecordsResponse>>;
}
