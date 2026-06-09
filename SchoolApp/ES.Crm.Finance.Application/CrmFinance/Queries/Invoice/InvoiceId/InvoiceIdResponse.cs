using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.CrmEnum;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.InvoiceId
{
    public record InvoiceIdResponse
    (
        string id = "",
            string invoiceNumber = "",
            string? applicantId = "",
            string? applicantName="",
            decimal? paidAmount=0,
            string? phoneNumber="",
            decimal totalAmount = 0,
            InvoiceStatus invoiceStatus = default,
            DateTime issueDate = default,
            DateTime? dueDate = null,
            List<InvoiceItemsDTOs> InvoiceItemsDTOs=default
        );
}
