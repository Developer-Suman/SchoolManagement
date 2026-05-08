using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Invoice.AddInvoice
{
    public record AddInvoiceCommand
    (
         string applicantId,
        decimal paidAmount,
        DateTime issueDate,
        DateTime? dueDate,
        List<AddInvoiceItemDTOs> addInvoiceItemDTOs
        ) : IRequest<Result<AddInvoiceResponse>>;
}
