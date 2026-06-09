using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ES.Crm.Finance.Application.CrmFinance.Command.Invoice.AddInvoice;
using static TN.Shared.Domain.Enum.CrmEnum;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Invoice.UpdateInvoice
{
    public record UpdateInvoiceResponse
    (
            string invoiceNumber="",
            string applicantId="",
            decimal totalAmount=0,
            decimal paidAmount=0,
            decimal dueAmount=0,
            InvoiceStatus invoiceStatus=default,
            DateTime issueDate=default,
            DateTime? dueDate=default,
            List<AddInvoiceItemDTOs> addInvoiceItemDTOs=default
    );
}
