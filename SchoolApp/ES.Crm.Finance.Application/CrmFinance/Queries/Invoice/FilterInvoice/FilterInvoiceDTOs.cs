using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.FilterInvoice
{
    public record FilterInvoiceDTOs
    (
        string? startDate,
        string? endDate,
        string? applicantId,
        string? invoiceNumber
        );
}
