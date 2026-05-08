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
        string invoiceNumber,
        string applicantId,
        decimal paidAmount,
        DateTime issueDate,
        DateTime? dueDate,
        List<AddInvoiceItemDTOs> addInvoiceItemDTOs
        ) : IRequest<Result<UpdateInvoiceResponse>>;
}
