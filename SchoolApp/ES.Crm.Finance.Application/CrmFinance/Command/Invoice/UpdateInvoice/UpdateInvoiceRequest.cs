using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ES.Crm.Finance.Application.CrmFinance.Command.Invoice.AddInvoice;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Invoice.UpdateInvoice
{
    public record UpdateInvoiceRequest
    (
         string invoiceNumber,
         string applicantId,
         decimal paidAmount,
         DateTime issueDate,
         DateTime? dueDate,
         List<AddInvoiceItemDTOs> addInvoiceItemDTOs
        );
}
