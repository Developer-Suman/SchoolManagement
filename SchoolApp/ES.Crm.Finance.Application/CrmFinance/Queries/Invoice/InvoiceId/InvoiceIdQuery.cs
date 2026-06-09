using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.InvoiceId
{
    public record InvoiceIdQuery
    (
        string id
        ) : IRequest<Result<InvoiceIdResponse>>;
}
