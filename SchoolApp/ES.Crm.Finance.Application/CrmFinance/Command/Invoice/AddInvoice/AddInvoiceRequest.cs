using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Invoice.AddInvoice
{
    public record AddInvoiceRequest
    (
        
        string applicantId,
        DateTime issueDate,
        DateTime? dueDate,
        bool isInstallments,
        List<AddInvoiceItemDTOs> addInvoiceItemDTOs

    );
}