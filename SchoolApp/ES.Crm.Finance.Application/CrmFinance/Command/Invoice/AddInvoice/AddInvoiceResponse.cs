using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.CrmEnum;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Invoice.AddInvoice
{
    public record AddInvoiceResponse
    (
        string id,
            string invoiceNumber,
            string applicantId,
            decimal totalAmount,
            decimal paidAmount,
            decimal dueAmount,
            InvoiceStatus invoiceStatus,
            DateTime issueDate,
            DateTime? dueDate
        );
}
