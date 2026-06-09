using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.CrmEnum;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.FilterInvoice
{
    public record FilterInvoiceResponse
    (
        string id="",
            string invoiceNumber="",
            string applicantName="",
            string? applicantId="",
            decimal totalAmount=0,
            InvoiceStatus invoiceStatus=default,
            DateTime issueDate=default,
            DateTime? dueDate=null,
            bool isActive=true,
            string schoolId="",
            string createdBy="",
            DateTime createdAt=default,
            string modifiedBy = "",
            DateTime modifiedAt=default
        );
}
