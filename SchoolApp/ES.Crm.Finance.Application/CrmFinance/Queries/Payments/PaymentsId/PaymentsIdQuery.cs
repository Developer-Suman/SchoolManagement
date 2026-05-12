using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Payments.PaymentsId
{
    public record PaymentsIdQuery
    (
        string id
        ) : IRequest<Result<PaymentsIdResponse>>;

}
