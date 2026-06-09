using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ES.Crm.Finance.Application.CrmFinance.Command.Invoice.AddInvoice;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Invoice.UpdateInvoice
{
    public record UpdateInvoiceCommand
    (
        string id,
        string applicantId,
        DateTime issueDate,
        DateTime? dueDate,
        List<UpdateInvoiceItemDTOs> updateInvoiceItemDTOs
        ) : IRequest<Result<UpdateInvoiceResponse>>;
}
